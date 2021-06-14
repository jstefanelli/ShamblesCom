import Game from "./Game";

export default interface Track {
	id: number;
	uuid: string;
	game: Game;
	name: string;
	country: string;
}