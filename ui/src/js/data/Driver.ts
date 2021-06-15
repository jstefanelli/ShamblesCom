import RaceResult from "./RaceResult";

export default interface Driver {
	id: number;
	uuid: string;
	nickname: string;
	raceResults: RaceResult[];
}