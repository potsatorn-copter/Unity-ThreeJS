from transformers import MarianMTModel, MarianTokenizer

# ดาวน์โหลดโมเดลและ tokenizer
model_name = 'Helsinki-NLP/opus-mt-en-de'
tokenizer = MarianTokenizer.from_pretrained(model_name)
model = MarianMTModel.from_pretrained(model_name)

# ฟังก์ชันสำหรับการแปลข้อความ
def translate(text):
    # เตรียมข้อความที่จะแปล
    inputs = tokenizer(text, return_tensors='pt', padding=True)
    # ทำการแปลข้อความ
    translated = model.generate(**inputs)
    # ถอดรหัสข้อความที่แปลแล้ว
    translated_text = [tokenizer.decode(t, skip_special_tokens=True) for t in translated]
    return translated_text

# ตัวอย่างการแปลข้อความ
if __name__ == "__main__":
    text = "Hello, how are you?"
    translation = translate(text)
    print("Translated text:", translation)