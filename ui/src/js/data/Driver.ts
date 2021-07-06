import RaceResult from "./RaceResult";

export default interface Driver {
	id: number;
	nickname: string;
	number: number;
	raceResults?: RaceResult[];
}