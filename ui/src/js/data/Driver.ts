import RaceResult from "./RaceResult";

export default interface Driver {
	id: number;
	nickname: string;
	raceResults?: RaceResult[];
}