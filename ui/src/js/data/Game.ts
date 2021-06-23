import Track from "./Track";

export default interface Game {
	id: number;
	name: string;
	description: string;
	tracks?: Track[];
}