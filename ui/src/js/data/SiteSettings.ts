import Race from "./Race";
import Season from "./Season";

export default interface SiteSettings {
	id: number;
	liveRaceId?: number;
	liveRace?: Race;
	liveStreamLinksArray: string[];
	currentHomeSeasonId?: number;
	currentHomeSeason?: Season;
}