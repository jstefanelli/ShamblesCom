import Game from "./Game";

export default interface Category {
	id: number;
	name: string;
	gameId: number;
	game: Game;
}