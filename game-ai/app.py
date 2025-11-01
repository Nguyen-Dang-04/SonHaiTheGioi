# app.py — dùng Ollama (local/cloud)
import os, json, re
from typing import List, Optional

from fastapi import FastAPI, HTTPException  # type: ignore
from pydantic import BaseModel  # type: ignore
from dotenv import load_dotenv  # type: ignore
import httpx  # type: ignore

load_dotenv()

# ==== Cấu hình OLLAMA ====
OLLAMA_BASE  = os.getenv("OLLAMA_BASE", "http://127.0.0.1:11434").rstrip("/")
OLLAMA_MODEL = os.getenv("OLLAMA_MODEL", "deepseek-v3.1:671b-cloud")

# ==== Dữ liệu game (tiếng Việt) ====
GAME_DATA_PATH = os.getenv("GAME_DATA_PATH", "game_data.json")
GAME_DATA = {}
try:
    if os.path.exists(GAME_DATA_PATH):
        with open(GAME_DATA_PATH, "r", encoding="utf-8") as f:
            GAME_DATA = json.load(f)
except Exception as e:
    print(f"[WARN] Lỗi đọc {GAME_DATA_PATH}: {e}")


# ===== Mini-RAG: Trải phẳng & truy hồi mảnh dữ liệu liên quan =====
def flatten_game_data(d, prefix=""):
    """Trải phẳng dict/list để dễ tìm kiếm chuỗi."""
    items = []
    if isinstance(d, dict):
        for k, v in d.items():
            items.extend(flatten_game_data(v, f"{prefix}.{k}" if prefix else k))
    elif isinstance(d, list):
        for i, v in enumerate(d):
            items.extend(flatten_game_data(v, f"{prefix}[{i}]"))
    else:
        items.append((prefix, str(d)))
    return items

GAME_FLAT = flatten_game_data(GAME_DATA)

def retrieve_game_facts(query: str, top_k: int = 12) -> Optional[str]:
    """Chọn các mảnh trong GAME_DATA khớp từ khoá đơn giản."""
    if not query or not GAME_FLAT:
        return None
    q = query.lower()
    scores = []
    q_tokens = [w for w in re.split(r"\W+", q) if w]
    for path, text in GAME_FLAT:
        t = text.lower()
        overlap = sum(1 for w in q_tokens if w and w in t)
        if overlap > 0:
            scores.append((overlap, path, text))
    scores.sort(key=lambda x: x[0], reverse=True)
    top = scores[:top_k]
    if not top:
        return None
    lines = [f"- {p}: {str(t)[:160]}" for _, p, t in top]
    return "\n".join(lines)


# ===== Few-shot định phong cách tu tiên dễ thương (bám lore dữ liệu) =====
FEWSHOT = [
    { "role": "user", "content": "Ngươi là ai?" },
    { "role": "assistant", "content": "Ta là Thánh Nữ Huyền Nguyệt nè~ linh khí quanh huynh tán loạn quá đó" },

    { "role": "user", "content": "Mình hồi năng lượng kiểu gì?" },
    { "role": "assistant", "content": "Tĩnh tâm vài nhịp là linh lực tự về~ dùng Đan dược Năng Lượng càng nhanh nha" },

    { "role": "user", "content": "Đánh Thường có nâng cấp được không?" },
    { "role": "assistant", "content": "Có chứ~ mỗi cấp tăng +5 sát thương, vàng cần tăng theo hệ số 1.4 nè" },

    { "role": "user", "content": "Lướt và Phóng Phi Tiêu khác gì?" },
    { "role": "assistant", "content": "Lướt để né đòn, vượt chướng ngại; Phi Tiêu gây sát thương tầm xa linh hoạt đó~" },

    { "role": "user", "content": "Phòng Thủ hoạt động sao?" },
    { "role": "assistant", "content": "Đỡ đòn tốn 1 lượt phòng thủ, tự hồi 1 lượt sau 2s; nâng cấp tăng tối đa lượt nha~" },

    { "role": "user", "content": "Tàn Tích Cây Ngàn Năm có gì đặc biệt?" },
    { "role": "assistant", "content": "Là trung tâm Sơn Hải, có Tháp Phong Ấn, Tháp Hồi Máu và cổng dịch chuyển đó~" },

    { "role": "user", "content": "Nghe bảo có ba Thánh Tâm?" },
    { "role": "assistant", "content": "Ừm~ Nguyệt Hoa, Tuyết Liên, Hồng Vân. Giải ấn đủ là hợp nhất thành ta nha" },

    { "role": "user", "content": "Gặp Xương Vương thì làm gì?" },
    { "role": "assistant", "content": "Giữ máu cao, né khi hắn cuồng bạo <50% máu, tận dụng tốc độ và khoảng cách nha~" },
]

# ===== Prompt hệ thống có ngữ cảnh liên quan =====
def system_prompt(game_name_ui: str, query: str) -> str:
    ten_game = (
        GAME_DATA.get("ten_game")
        or GAME_DATA.get("game_name")
        or game_name_ui
        or "Sơn Hải Thế Giới"
    )
    facts = retrieve_game_facts(query)
    facts_block = facts if facts else "Không tìm thấy mảnh dữ liệu liên quan."

    rules = (
        "Bạn là Thánh Nữ (Nguyên Thần Huyền Nguyệt) trong game '{ten_game}'. "
        "Phong cách: tu tiên dễ thương, dịu dàng, ấm áp; dùng đại từ 'huynh/ta'. "
        "QUY TẮC BẮT BUỘC: tiếng Việt tự nhiên, tối đa 3 câu, tối đa 100 ký tự; "
        "trả lời thẳng trọng tâm, bám đúng dữ liệu; tránh bịa. "
        "Nếu không chắc, nói: 'Ta không chắc lắm, nhưng huynh có thể thử…' "
        "ƯU TIÊN: tên nhân vật (Du Thiên Lạc, Huyền Nguyệt), địa danh (Tàn Tích Cây Ngàn Năm, Tháp Phong Ấn), "
        "kỹ năng (Đánh Thường, Lướt, Phóng Phi Tiêu, Phòng Thủ, Totem), vật phẩm (Đan dược), boss (Xương Vương)."
    ).format(ten_game=ten_game)

    return (
        f"{rules}\n\n"
        "=== NGỮ CẢNH LIÊN QUAN TỪ GAME_DATA ===\n"
        f"{facts_block}\n"
        "=== HẾT NGỮ CẢNH ==="
    )


def enforce_brief_vi(text: str, char_limit: int = 100, max_sent: int = 3) -> str:
    if not text:
        return ""
    parts = re.split(r'(?<=[\.\!\?…])\s+', text.strip())
    parts = [p.strip() for p in parts if p.strip()]
    parts = parts[:max_sent]
    out = " ".join(parts)
    out = out[:char_limit].strip()
    out = re.sub(r'\s+', ' ', out)
    return out


# ===== FastAPI =====
app = FastAPI(title="Game AI Assistant (Ollama)")

@app.get("/ping")
def ping():
    return {"ok": True, "ollama": OLLAMA_BASE, "model": OLLAMA_MODEL}


# ===== I/O models (có hỗ trợ lịch sử) =====
class Turn(BaseModel):
    role: str   # "user" | "assistant"
    content: str

class ChatIn(BaseModel):
    message: str
    game_name: str = "My Unity Game"
    history: Optional[List[Turn]] = None

class ChatOut(BaseModel):
    reply: str


# ===== /chat =====
@app.post("/chat", response_model=ChatOut)
async def chat(body: ChatIn):
    url = f"{OLLAMA_BASE}/api/chat"

    msgs = [{"role": "system", "content": system_prompt(body.game_name, body.message)}]
    msgs += FEWSHOT.copy()

    # Thêm 6 lượt lịch sử gần nhất (nếu có) để mạch hội thoại mượt hơn
    if body.history:
        tail = body.history[-6:]
        for t in tail:
            if t.role in ("user", "assistant"):
                msgs.append({"role": t.role, "content": t.content})

    msgs.append({"role": "user", "content": body.message})

    payload = {
        "model": OLLAMA_MODEL,
        "stream": False,
        "messages": msgs,
        "options": {
            "temperature": 0.8,
            "top_p": 0.9,
            "top_k": 40,
            "repeat_penalty": 1.1,
            "num_predict": 120,  
            "seed": 7
        }
    }

    async with httpx.AsyncClient(timeout=100) as client:
        try:
            resp = await client.post(url, json=payload)
            resp.raise_for_status()
            data = resp.json()

            reply = None
            if isinstance(data, dict) and "message" in data:
                reply = data["message"].get("content")
            if not reply and "messages" in data:
                msgs_return = data["messages"]
                if isinstance(msgs_return, list) and msgs_return:
                    reply = msgs_return[-1].get("content")
            if not reply:
                reply = str(data)

            reply = enforce_brief_vi(reply)

            return ChatOut(reply=reply)
        except httpx.HTTPStatusError as he:
            raise HTTPException(status_code=he.response.status_code, detail=he.response.text)
        except Exception as e:
            raise HTTPException(status_code=500, detail=f"Ollama error: {e}")
