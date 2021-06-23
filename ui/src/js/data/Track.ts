import Game from "./Game";
import Race from "./Race";

export default interface Track {
	id: number;
	game: Game;
	name: string;
	country: string;
	races?: Race[];
}