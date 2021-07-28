import Driver from "./Driver";
import Season from "./Season";

export default interface DriverProfile {
	id: number;
	driverId: number;
	driver?: Driver;
	seasonId: number;
	season?: Season;
	shortDescription: string;
	description: string;
	imageLink: string;
}