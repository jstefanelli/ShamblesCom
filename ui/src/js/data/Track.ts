import Game from "./Game";
import Race from "./Race";

export default interface Track {
	id: number;
	uuid: string;
	game: Game;
	name: string;
	country: string;
	races: Race[];
}