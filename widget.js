(() => {
    const scriptTag = document.currentScript;
    const endpoint = (scriptTag && scriptTag.dataset.endpoint) || window.location.origin;
    const UserId = localStorage.getItem("chatUserId") || crypto.randomUUID();
    localStorage.setItem("chatUserId", UserId);

    const container = document.createElement("div");
    container.innerHTML = `
<style>
  #chat-toggle {
    position: fixed; bottom: 20px; right: 20px;
    width: 60px; height: 60px; border-radius: 50%;
    background: #008080; color: white; font-size: 30px;
    display: flex; align-items: center; justify-content: center;
    cursor: pointer; box-shadow: 0 4px 12px rgba(0,0,0,0.2);
  }
  #chat-widget {
    position: fixed; bottom: 90px; right: 20px;
    width: 400px; height: 500px; border-radius: 10px;
    box-shadow: 0 4px 12px rgba(0,0,0,0.2);
    background-color: #f9f9f9;
    background-image: url('https://www.transparenttextures.com/patterns/cubes.png');
    background-repeat: repeat;
    display: none; flex-direction: column;
    font-family: Arial, sans-serif; overflow: hidden;
  }
  #chat-header {
    background: #008080; color: white; padding: 10px;
    font-weight: bold; text-align: center;
  }
  #chat-messages {
    flex: 1; padding: 10px; overflow-y: auto;
  }
  .message {
    margin: 5px 0; padding: 8px 12px;
    border-radius: 20px; max-width: 70%;
    word-wrap: break-word; font-size: 14px;
  }
  .message.user {
    background: #B0E0E6; color: #333; margin-left: auto;
  }
  .message.agent {
    background: #20B2AA; color: #333; margin-right: auto;
  }
  #chat-input-container {
    display: flex; align-items: center;
    border-top: 1px solid #ccc; padding: 5px;
    gap: 5px;
  }
  #chat-input {
    flex: 1; border: 1px solid #ccc;
    border-radius: 5px; padding: 8px;
    font-size: 14px;
  }
  #chat-input:focus { outline: none; }
  #chat-send {
    background: #008080; color: white; border: none;
    padding: 0 15px; cursor: pointer; font-size: 14px;
    height: 34px; border-radius: 5px;
  }
  
  #chat-file-btn {
  width: 34px; height: 34px;
  background:#008080; color: white;
  border: none; border-radius: 5px;
  cursor: pointer; font-size: 18px;
   }
  #chat-file-btn:hover {
    background: #005f5f;
   }
  #imageModal {
    display: none; position: fixed; z-index: 9999;
    left: 0; top: 0; width: 100%; height: 100%;
    background-color: rgba(0,0,0,0.8);
    justify-content: center; align-items: center;
  }
  #modalImage {
    max-width: 90%; max-height: 90%;
    border-radius: 10px; object-fit: contain;
  }
  #closeModalBtn {
    position: absolute; top: 20px; right: 20px;
    background: #fff; border: none;
    border-radius: 50%; width: 35px; height: 35px;
    cursor: pointer; font-size: 20px; z-index: 10000;
  }

  /* Responsive */
  @media (max-width: 500px) {
    #chat-widget { width: 90%; height: 60%; }
  }
</style>

<div id="chat-toggle">💬</div>

<div id="chat-widget">
  <div id="chat-header">Chat</div>
  <div id="chat-messages"></div>
  <div id="chat-input-container">
    <input id="chat-input" placeholder="Mesaj yaz..." />
    <button id="chat-file-btn" title="Dosya gönder">📎</button>
    <input id="chat-file" type="file" style="display:none;" />
    <button id="chat-send">Gönder</button>
  </div>
</div>

<div id="imageModal">
  <button id="closeModalBtn">✖</button>
  <img id="modalImage" />
</div>
`;

    document.body.appendChild(container);

    const chatToggle = document.getElementById("chat-toggle");
    const chatWidget = document.getElementById("chat-widget");
    const messagesDiv = document.getElementById("chat-messages");
    const input = document.getElementById("chat-input");
    const sendBtn = document.getElementById("chat-send");
    const fileInput = document.getElementById("chat-file");
    const modal = document.getElementById("imageModal");
    const modalImg = document.getElementById("modalImage");
    const closeModalBtn = document.getElementById("closeModalBtn");
    const fileBtn = document.getElementById("chat-file-btn");
    fileBtn.addEventListener("click", () => fileInput.click());


    // Chat penceresini açıp kapama işlevi
    chatToggle.addEventListener("click", () => {
        chatWidget.style.display = chatWidget.style.display === "flex" ? "none" : "flex";
    });

    //Mesajı sohbet penceresine ekler ve türüne göre (metin, resim, dosya) görüntülenmesini sağlar.
    function appendMessage(msg, from) {
        const div = document.createElement("div");
        div.className = "message " + from;

        if (msg.type === "file" || msg.type === "image") {
            if (msg.type === "image" && msg.url) {
                const img = document.createElement("img");
                img.src = msg.url;
                img.style.maxWidth = "200px";
                img.style.borderRadius = "10px";
                img.style.cursor = "pointer";
                img.onclick = () => {
                    modal.style.display = "flex";
                    modalImg.src = msg.url;
                };
                div.appendChild(img);
            } else {
                const a = document.createElement("a");
                a.href = msg.url;
                a.textContent = msg.content;
                a.target = "_blank";
                div.appendChild(a);
            }
        } else {
            div.innerText = msg.content;
        }

        messagesDiv.appendChild(div);
        messagesDiv.scrollTop = messagesDiv.scrollHeight; // otomatik scroll
    }

    closeModalBtn.onclick = () => { modal.style.display = "none"; };

    // SignalR bağlantısını kurar ve otomatik yeniden bağlanmayı etkinleştirir.
    const connection = new signalR.HubConnectionBuilder()
        .withUrl(endpoint.replace(/\/$/, "") + "/chathub")
        .withAutomaticReconnect()
        .build();

    // Sunucudan gelen mesajları dinler ve ekrana ekler.
    connection.on("ReceiveMessage", (message) => {
        if (message.userId === UserId && message.agentId) {
            appendMessage(message, "agent");
        }
    });

    // SignalR bağlantısını başlatır ve kullanıcı olarak kayıt olur.
    connection.start().then(() => {
        connection.invoke("RegisterAsUser", UserId);
        console.log("Widget connected as user:", UserId);


        // API üzerinden geçmiş mesajları alır
        fetch(`/api/messages/${UserId}`)
            .then(res => res.json())
            .then(messages => {
                messages.forEach(msg => appendMessage(msg, msg.agentId ? "agent" : "user"));
            });
    });


    // Mesaj gönderme düğmesine tıklama olayını yönetir.
    sendBtn.onclick = async () => {
        const content = input.value.trim();
        if (!content) return;
        const temp = { content, type: "text", userId: UserId };
        appendMessage(temp, "user");

        connection.invoke("SendFromUser", UserId, content, new Date(), "text", null);
        input.value = "";
    };

    // Dosya gönderme olayını yönetir.
    fileInput.addEventListener("change", async () => {
        const file = fileInput.files[0];
        if (!file) return;

        const formData = new FormData();
        formData.append("file", file);
        formData.append("userId", UserId);

        const res = await fetch(`${endpoint}/api/messages/upload`, {
            method: "POST",
            body: formData
        });

        const message = await res.json();
        appendMessage(message, "user");
        connection.invoke("SendFromUser", UserId, message.content, new Date(), message.type, message.url);
        fileInput.value = "";
    });

    // Kullanıcı yazmaya başladığında sunucuya "yazıyor" bilgisini gönderir.
    input.addEventListener("input", () => { connection.invoke("UserTyping", UserId); });

    // Operatörün "yazıyor" bilgisini dinler ve ekranda gösterir.
    connection.on("AgentTyping", () => {
        let typingDiv = document.querySelector("#chat-messages .typing-indicator");
        if (!typingDiv) {
            typingDiv = document.createElement("div");
            typingDiv.className = "typing-indicator";
            typingDiv.style.fontStyle = "italic";
            typingDiv.style.fontSize = "12px";
            typingDiv.style.color = "#666";
            messagesDiv.appendChild(typingDiv);
        }
        typingDiv.textContent = "Operatör yazıyor...";
        clearTimeout(typingDiv.timeout);
        typingDiv.timeout = setTimeout(() => typingDiv.textContent = "", 2000);
    });

})();
