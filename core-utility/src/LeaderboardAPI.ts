import { LeaderboardHandler } from './LeaderboardHandler';
import { LeaderboardData } from './types';
import axios from 'axios';


const API_URL = 'http://localhost:5000';

class LeaderBoardAPI implements LeaderboardHandler {
    async getLeaderboard(): Promise<LeaderboardData[]> {
        try {
          const response = await axios.get(`${API_URL}/leaderboard`);
          return response.data;
        } catch (error) {
          console.error('Error fetching leaderboard:', error);
          return [];
        }
      }

      async updateLeaderboard(playerName: string, score: number, character: string): Promise<void> {
        try {
          await axios.post(`${API_URL}/leaderboard`, { name: playerName, score, character });
        } catch (error) {
          console.error('Error updating leaderboard:', error);
        }
      }

      async resetLeaderboard(): Promise<void> {
        try {
          await axios.delete(`${API_URL}/leaderboard`);
        } catch (error) {
          console.error('Error resetting leaderboard:', error);
        }
      }

      async isNameDuplicate(name: string): Promise<boolean> {
        try {
          const leaderboard = await this.getLeaderboard();
          return leaderboard.some(entry => entry.name === name);
        } catch (error) {
          console.error('Error checking for duplicate name:', error);
          return false;
        }
      }
    
      }
      export default LeaderBoardAPI;