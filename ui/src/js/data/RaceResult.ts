import Driver from "./Driver";
import Team from "./Team";
import Race from "./Race";

export default interface RaceResult {
	id: number;
	driverId: number;
	driver?: Driver;
	teamId: number;
	team?: Team;
	raceId: number;
	race?: Race;
	position: number;
	finished: boolean;
	penalties: number;
	startPosition: number;
	points: number;
}