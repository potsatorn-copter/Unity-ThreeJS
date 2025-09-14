from flask import Flask, request, jsonify
from transformers import MBartForConditionalGeneration, MBart50TokenizerFast

app = Flask(__name__)

# ดาวน์โหลดโมเดลและ tokenizer
model_name = 'facebook/mbart-large-50-many-to-many-mmt'
tokenizer = MBart50TokenizerFast.from_pretrained(model_name)
model = MBartForConditionalGeneration.from_pretrained(model_name)

# กำหนดภาษาต้นทางและภาษาปลายทาง
src_lang = 'en_XX'  # ภาษาอังกฤษ
tgt_lang = 'th_TH'  # ภาษาไทย

# ฟังก์ชันสำหรับการแปลข้อความ
def translate(text):
    try:
        # ตั้งค่าภาษาใน tokenizer
        tokenizer.src_lang = src_lang
        inputs = tokenizer(text, return_tensors='pt')
        generated_tokens = model.generate(**inputs, forced_bos_token_id=tokenizer.lang_code_to_id[tgt_lang])
        translated_text = tokenizer.batch_decode(generated_tokens, skip_special_tokens=True)
        return translated_text[0]
    except Exception as e:
        print(f"Error during translation: {e}")
        return None

@app.route('/translate', methods=['POST'])
def translate_route():
    data = request.json
    text = data['text']
    translated_text = translate(text)
    if translated_text:
        return jsonify({'translated_text': translated_text})
    else:
        return jsonify({'error': 'Translation failed'}), 500

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5000)