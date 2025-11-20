class Chatbot {
    constructor() {
        this.isOpen = false;
        this.storageKey = 'chatSessionId';
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
                    Fitness Assistant
                </div>
                <div class="chatbot-messages" id="chatbotMessages">
                    <div class="message bot">
                        Hi! I'm your fitness assistant. How can I help you today?
                    </div>
                </div>
                <div class="chatbot-input">
                    <input type="text" id="chatbotInput" placeholder="Type your message...">
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
        const sessionId = localStorage.getItem(this.storageKey);
        const messagesContainer = document.getElementById('chatbotMessages');

        if (!sessionId) {
            return;
        }

        // show a simple loading indicator
        const loadingDiv = document.createElement('div');
        loadingDiv.className = 'message bot loading';
        loadingDiv.textContent = 'Loading conversation…';
        messagesContainer.appendChild(loadingDiv);
        messagesContainer.scrollTop = messagesContainer.scrollHeight;

        try {
            const response = await fetch(`/Chat/Session/${encodeURIComponent(sessionId)}`, {
                method: 'GET',
                headers: { 'Accept': 'application/json' }
            });

            if (!response.ok) {
                const text = await response.text();
                loadingDiv.textContent = `Error loading history: ${text}`;
                return;
            }

            const data = await response.json();

            // clear existing messages and render history
            messagesContainer.innerHTML = '';
            if (!Array.isArray(data) || data.length === 0) {
                messagesContainer.innerHTML = `<div class="message bot">Hi! I'm your fitness assistant. How can I help you today?</div>`;
                return;
            }

            data.forEach(m => {
                const sender = m.isUserMessage ? 'user' : 'bot';
                this.addMessage(m.message, sender);
            });
        } catch (err) {
            this.addMessage(`Error loading history: ${err.message}`, 'bot');
        } finally {
            // ensure loading indicator removed if still present
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
            const sessionId = localStorage.getItem(this.storageKey);
            const payload = {
                sessionId: sessionId ? sessionId : null,
                message: message
            };

            const response = await fetch('/Chat/SendMessage', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(payload)
            });

            const data = await response.json();

            if (data && data.success) {
                if (data.sessionId) {
                    localStorage.setItem(this.storageKey, data.sessionId);
                }
                this.addMessage(data.assistant, 'bot');
            } else {
                const err = (data && data.error) ? data.error : 'Unknown error';
                this.addMessage(`Error: ${err}`, 'bot');
            }
        } catch (error) {
            this.addMessage(`Error: ${error.message}`, 'bot');
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
