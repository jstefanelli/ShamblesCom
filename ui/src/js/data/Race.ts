import RaceResult from "./RaceResult";
import Season from "./Season";
import Track from "./Track";

export enum RaceType {
	MainShambles,
	F2Shambles,
	Other
}

export default interface Race {
	id: number;
	uuid: string;
	dateTime: string;
	seasonId: number;
	season: Season;
	trackId: number;
	track: Track;
	identifier: string
	fullName: string;

	raceResults: RaceResult[];
}

export class RaceClass {
	public type: RaceType = RaceType.MainShambles;
	public country: string = "spain";
	public time: Date = new Date('2021-12-1 21:30');

	public toString(): string {
		let ctr = this.country;
		ctr.replace(/^\w/, (c) => c.toUpperCase());

		let race_type = "SGP";
		switch(this.type) {
			case RaceType.MainShambles:
				race_type = "SGP";
				break;
			case RaceType.F2Shambles:
				race_type = "Super Weenie Hut GP";
				break;
			case RaceType.Other:
				race_type = "casual race"
				break;
		}

		return ctr + " " + race_type;
	}

	public isShambles() : boolean {
		return this.type == RaceType.MainShambles;
	}

	public isF2() : boolean {
		return this.type == RaceType.F2Shambles;
	}

	public isCasual() : boolean {
		return this.type == RaceType.Other;
	}

	public capitalCountry(): string {
		let ctr = this.country;
		return ctr.replace(/^\w/, (c) => c.toUpperCase());
	}
}