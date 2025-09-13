import axios from 'axios';  // ไลบรารี JavaScript ที่ใช้สำหรับทำ HTTP requests เพื่อดึง, ส่ง, อัพเดต หรือทำการลบข้อมูลจากเซิร์ฟเวอร์ สามารถใช้ได้ทั้งในเบราว์เซอร์และ Node.js
import { LeaderboardData } from './types';

const API_URL = 'http://localhost:5000';

// สร้างคลาส สำหรับการจัดการกับ leaderboarder
export interface LeaderboardHandler {
    // ดึงข้อมูล leaderboard จาก API
    getLeaderboard(): Promise<LeaderboardData[]> 

  
 // อัปเดต leaderboard โดยเพิ่มข้อมูลใหม่
   updateLeaderboard(playerName: string, score: number, character: string): Promise<void> 
    
  
 // รีเซ็ต leaderboard โดยการลบข้อมูลทั้งหมด
   resetLeaderboard(): Promise<void> 
  
  
 // ตรวจสอบว่าชื่อผู้เล่นซ้ำกันหรือไม่
   isNameDuplicate(name: string): Promise<boolean> 
    
  

  }
