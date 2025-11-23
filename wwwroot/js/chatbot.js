class Chatbot {
    constructor() {
        this.isOpen = false;
        this.init();
    }

    init() {
        this.createWidget();
        this.bindEvents();
    }

    createWidget() {
        const widget = document.createElement('div');
        widget.className = 'chatbot-widget';
        widget.innerHTML = `
            <div class="chatbot-container" id="chatbotContainer" style="display:none;">
                <div class="chatbot-header">
                    Träningsassistent
                </div>
                <div class="chatbot-messages" id="chatbotMessages">
                    <div class="message bot">
                        Hej! Jag är din träningsassistent. Hur kan jag hjälpa dig idag?
                    </div>
                </div>
                <div class="chatbot-input">
                    <input type="text" id="chatbotInput" placeholder="Skriv ditt meddelande...">
                    <button id="chatbotSend">➤</button>
                </div>
            </div>
            <button class="chatbot-toggle" id="chatbotToggle">💬</button>
        `;
        document.body.appendChild(widget);
    }

    bindEvents() {
        const toggle = document.getElementById('chatbotToggle');
        const sendBtn = document.getElementById('chatbotSend');
        const input = document.getElementById('chatbotInput');

        toggle.addEventListener('click', () => this.toggleChat());
        sendBtn.addEventListener('click', () => this.sendMessage());
        input.addEventListener('keypress', (e) => {
            if (e.key === 'Enter') this.sendMessage();
        });
    }

    toggleChat() {
        const container = document.getElementById('chatbotContainer');
        this.isOpen = !this.isOpen;
        container.style.display = this.isOpen ? 'flex' : 'none';

        if (this.isOpen) {
            this.loadHistoryIfAny();
        }
    }

    async loadHistoryIfAny() {
        const messagesContainer = document.getElementById('chatbotMessages');

        // show a simple loading indicator
        messagesContainer.innerHTML = '';
        const loadingDiv = document.createElement('div');
        loadingDiv.className = 'message bot loading';
        loadingDiv.textContent = 'Läser in konversation…';
        messagesContainer.appendChild(loadingDiv);
        messagesContainer.scrollTop = messagesContainer.scrollHeight;

        try {
            const response = await fetch('/Chat/Messages', {
                method: 'GET',
                headers: { 'Accept': 'application/json' },
                credentials: 'same-origin' // forward cookies if api uses cookie auth
            });

            if (!response.ok) {
                const text = await response.text();
                loadingDiv.textContent = `Fel vid inläsning av historik: ${text}`;
                return;
            }

            const data = await response.json();

            // clear existing messages and render history
            messagesContainer.innerHTML = '';
            if (!Array.isArray(data) || data.length === 0) {
                messagesContainer.innerHTML = `<div class="message bot">Hej! Jag är din träningsassistent. Hur kan jag hjälpa dig idag?</div>`;
                return;
            }

            data.forEach(m => {
                const sender = m.isUserMessage ? 'user' : 'bot';
                this.addMessage(m.message, sender);
            });
        } catch (err) {
            this.addMessage(`Fel vid inläsning av historik: ${err.message}`, 'bot');
        } finally {
            const loading = messagesContainer.querySelector('.loading');
            if (loading) loading.remove();
        }
    }

    async sendMessage() {
        const input = document.getElementById('chatbotInput');
        const sendBtn = document.getElementById('chatbotSend');
        const message = input.value.trim();
        if (!message) return;

        this.addMessage(message, 'user');
        input.value = '';
        input.disabled = true;
        sendBtn.disabled = true;

        try {
            const payload = { message };

            const response = await fetch('/Chat/SendMessage', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Accept': 'application/json'
                },
                credentials: 'same-origin', // forward cookies or same-origin auth
                body: JSON.stringify(payload)
            });

            const data = await response.json();

            if (data && data.success) {
                this.addMessage(data.assistant, 'bot');
            } else {
                const err = (data && data.error) ? data.error : 'Okänt fel';
                this.addMessage(`Fel: ${err}`, 'bot');
            }
        } catch (error) {
            this.addMessage(`Fel: ${error.message}`, 'bot');
        } finally {
            input.disabled = false;
            sendBtn.disabled = false;
            input.focus();
        }
    }

    addMessage(text, sender) {
        const messagesContainer = document.getElementById('chatbotMessages');
        const messageDiv = document.createElement('div');
        messageDiv.className = `message ${sender}`;
        messageDiv.textContent = text;
        messagesContainer.appendChild(messageDiv);
        messagesContainer.scrollTop = messagesContainer.scrollHeight;
    }
}

document.addEventListener('DOMContentLoaded', () => {
    new Chatbot();
});
