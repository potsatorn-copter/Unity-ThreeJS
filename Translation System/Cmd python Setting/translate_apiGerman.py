from flask import Flask, request, jsonify
from transformers import MarianMTModel, MarianTokenizer

app = Flask(__name__)

# ดาวน์โหลดโมเดลและ tokenizer
model_name = 'Helsinki-NLP/opus-mt-en-de'
tokenizer = MarianTokenizer.from_pretrained(model_name)
model = MarianMTModel.from_pretrained(model_name)

# ฟังก์ชันสำหรับการแปลข้อความ
def translate(text):
    inputs = tokenizer(text, return_tensors='pt', padding=True)
    translated = model.generate(**inputs)
    translated_text = [tokenizer.decode(t, skip_special_tokens=True) for t in translated]
    return translated_text

@app.route('/translate', methods=['POST'])
def translate_route():
    data = request.json
    text = data['text']
    translated_text = translate(text)
    return jsonify({'translated_text': translated_text[0]})

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5000)