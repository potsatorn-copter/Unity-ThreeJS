import { LeaderboardHandler } from './LeaderboardHandler';
import { LeaderboardData } from './types';

class LeaderboardLocalStorage implements LeaderboardHandler {
  private storageKey = 'leaderboard';

  // ดึงข้อมูลจาก Local Storage
  async getLeaderboard(): Promise<LeaderboardData[]> {
    const data = localStorage.getItem(this.storageKey);
    console.log('Fetched data from Local Storage:', data);
    return data ? JSON.parse(data) : [];
  }

  // อัพเดทข้อมูลใน Local Storage
  async updateLeaderboard(playerName: string, score: number, character: string): Promise<void> {
    const leaderboard = await this.getLeaderboard();

    // เพิ่มข้อมูลใหม่เสมอ ไม่ทับข้อมูลเดิม
    leaderboard.push({ name: playerName, score, character });

    localStorage.setItem(this.storageKey, JSON.stringify(leaderboard));
    console.log('Updated Local Storage with:', leaderboard);
  }

  // รีเซ็ตข้อมูลใน Local Storage
  async resetLeaderboard(): Promise<void> {
    localStorage.removeItem(this.storageKey);
  }

  // ตรวจสอบชื่อผู้เล่นซ้ำใน Local Storage
  async isNameDuplicate(name: string): Promise<boolean> {
    try {
      const leaderboard = await this.getLeaderboard();
      return leaderboard.some(entry => entry.name === name);
    } catch (error) {
      console.error('Error checking duplicate name in Local Storage:', error);
      return false;
    }
  }
}

export default LeaderboardLocalStorage;