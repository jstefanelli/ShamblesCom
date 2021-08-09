import Driver from './Driver';
import DriverProfile from './DriverProfile';

export default interface DriverInfo {
    driver: Driver;
    seasonPoints: number;
    seasonPosition: number;
    profile: DriverProfile;
}