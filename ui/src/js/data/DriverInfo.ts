import Driver from './Driver';
import DriverProfile from './DriverProfile';
import Team from './Team';

export default interface DriverInfo {
    driver: Driver;
    seasonPoints: number;
    seasonPosition: number;
    profile: DriverProfile;
    mostCommonTeam: Team;
}