# 🎮 Sơn Hải Thế Giới

Dự án game hành động 2D pixel-art kết hợp hệ thống AI hội thoại thông minh.

---

## 🕹️ UnityProject
- **Engine:** Unity 2022+  
- **Thư mục:** `UnityProject/`  
- **Mô tả:** Chứa gameplay, nhân vật, hệ thống NPC, hội thoại BoxChatAI, kỹ năng, map v.v.

---

## 🤖 game-ai (FastAPI Backend)
- **Ngôn ngữ:** Python 3.11+  
- **Framework:** FastAPI  
- **Thư mục:** `game-ai/`  
- **Chức năng:** Xử lý hội thoại AI giữa người chơi và NPC trong game.  

---

## 🧠 Cách chạy backend
> Thực hiện **Lần đầu tiên** khi bạn mới clone repo hoặc vừa cài lại Python

```bash
cd D:\DuAnUnity\SonHaiTheGioi\game-ai
python -m venv .venv
.\.venv\Scripts\Activate.ps1
pip install -r requirements.txt
python -m uvicorn app:app --reload --port 8000
```
> Thực hiện **Những lần sau** để chạy

```bash
cd D:\DuAnUnity\SonHaiTheGioi\game-ai
.\.venv\Scripts\Activate.ps1
python -m uvicorn app:app --reload --port 8000  
```bash
