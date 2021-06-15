import Game from "./Game";

export default interface Category {
	id: number;
	uuid: string;
	name: string;
	gameId: number;
	game: Game;
}