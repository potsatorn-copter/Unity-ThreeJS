const express = require('express'); // สร้าง Server
const fs = require('fs'); // File System โมดูลใน Node.js ที่ใช้สำหรับจัดการไฟล์และไดเรกทอรีต่างๆ บนระบบไฟล์ของเซิร์ฟเวอร์ มีฟังก์ชันหลากหลายที่ช่วยให้เราสามารถอ่าน, เขียน, ลบ, และแก้ไขไฟล์ได้
const cors = require('cors');  // Cross-Origin Resource Sharing เป็นกลไกที่ใช้ในการกำหนดว่าทรัพยากรในเว็บไซต์หนึ่งสามารถถูกเข้าถึงได้โดยหน้าเว็บจากต้นทางต่างๆ หรือไม่ 
const path = require('path');

const app = express();
const PORT = 5000;

app.use(cors());  // อนุญาตให้ทุกโดเมนสามารถทำการเรียกใช้งาน API ถ้ามีโดเมนของเราเองแล้วค่อยมาแก้ใหม่เพื่อจำกัดโดเมนที่เข้าถึง API 
app.use(express.json());

//การกำหนดเส้นทางไฟล์สำหรับเก็บข้อมูล leaderboard
const leaderboardFilePath = path.join(__dirname, 'leaderboard.json');

// โหลดข้อมูลจากไฟล์เมื่อเริ่มต้นเซิร์ฟเวอร์
let leaderboardData = [];
if (fs.existsSync(leaderboardFilePath)) {
  const rawData = fs.readFileSync(leaderboardFilePath);
  leaderboardData = JSON.parse(rawData);
}

// ฟังก์ชันสำหรับบันทึกข้อมูลลงไฟล์
const saveLeaderboardData = () => {
  fs.writeFileSync(leaderboardFilePath, JSON.stringify(leaderboardData, null, 2));
};

// ดึงข้อมูล leaderboard
app.get('/leaderboard', (req, res) => {
  try {
    res.json(leaderboardData);
  } catch (error) {
    console.error('Error fetching leaderboard:', error);
    res.sendStatus(500);
  }
});

// อัปเดต leaderboard
app.post('/leaderboard', (req, res) => {
  try {
    const { name, score, character } = req.body;
    if (!name || !score || !character) {
      return res.sendStatus(400); // Bad Request
    }
    leaderboardData.push({ name, score, character });
    saveLeaderboardData();
    res.sendStatus(200); // OK
  } catch (error) {
    console.error('Error updating leaderboard:', error);
    res.sendStatus(500); // Internal Server Error
  }
});

// รีเซ็ต leaderboard
app.delete('/leaderboard', (req, res) => {
  try {
    leaderboardData = [];
    saveLeaderboardData();
    res.sendStatus(200); // OK สำหรับการร้องขอที่สำเร็จ 400 (Bad Request) สำหรับการร้องขอที่มีข้อมูลไม่ถูกต้อง 404 (Not Found) สำหรับการร้องขอที่ไม่พบทรัพยากร   
  } catch (error) {
    console.error('Error resetting leaderboard:', error);
    res.sendStatus(500); // Internal Server Error  สำหรับข้อผิดพลาดภายในเซิร์ฟเวอร์
  } 
});

app.listen(PORT, () => {
  console.log(`Server is running on http://localhost:${PORT}`);
});