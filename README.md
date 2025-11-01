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
- **Cách chạy backend:**
  ```bash
  cd game-ai
  python -m venv .venv
  .venv\Scripts\activate
  pip install -r requirements.txt
  uvicorn app:app --reload
