import RaceResult from "./RaceResult";
import Season from "./Season";
import Track from "./Track";


export default interface Race {
	id: number;
	dateTime: string;
	seasonId: number;
	season: Season;
	trackId: number;
	track: Track;
	identifier: string;
	livestreamLink: string;
	vodLink: string;

	raceResults?: RaceResult[];
}