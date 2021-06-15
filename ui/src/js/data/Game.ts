import Track from "./Track";

export default interface Game {
	id: number;
	uuid: string;
	name: string;
	description: string;
	tracks: Track[];
}