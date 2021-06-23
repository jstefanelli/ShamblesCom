import RaceResult from "./RaceResult";
import Season from "./Season";

export default interface Team {
	id: number;
	name: string;
	mainColor: string;
	secondaryColor: string;
	seasonId: number;
	season: Season;
	raceResults?: RaceResult[];
}