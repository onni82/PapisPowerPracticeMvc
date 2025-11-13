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
            <div class="chatbot-container" id="chatbotContainer">
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
        const container = document.getElementById('chatbotContainer');
        const input = document.getElementById('chatbotInput');
        const sendBtn = document.getElementById('chatbotSend');

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
    }

    async sendMessage() {
        const input = document.getElementById('chatbotInput');
        const message = input.value.trim();
        
        if (!message) return;

        this.addMessage(message, 'user');
        input.value = '';

        try {
            const formData = new FormData();
            formData.append('message', message);
            
            const response = await fetch('/Chat/SendMessage', {
                method: 'POST',
                body: formData
            });

            const data = await response.json();
            
            if (data.success) {
                this.addMessage(data.response, 'bot');
            } else {
                this.addMessage(`Error: ${data.error}`, 'bot');
            }
        } catch (error) {
            this.addMessage(`Error: ${error.message}`, 'bot');
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