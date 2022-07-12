<template>
	<div class="flex items-center text-lg gap-3">
		<div class="flex flex-grow items-center gap-3 mh-3">
			<span>Day:</span>
			<Myinput type="date" :class="'text-sm ' + (dayError ? ' error' : '')" v-model="day"></Myinput>
		</div>
		<div class="flex flex-grow-[3] items-center gap-3 mh-3">
			<span>Hour:</span>
			<Myinput type="number" :class="'text-sm' + (hourError ? ' error' : '')" v-model.number="hour"></Myinput>

			<span>Minute:</span>
			<Myinput type="number" :class="'text-sm' + (minuteError ? ' error' : '')" v-model.number="minute" ></Myinput>
		</div>
	</div>
</template>

<script lang="ts">
import { Vue, Component, Prop, Watch } from 'vue-property-decorator';
import Myinput from './myinput.vue';


@Component({
	components: {
		Myinput
	}
})
export default class extends Vue {
	public day: string = '';
	public hour: number = 0;
	public minute: number = 0;

	public dayError: boolean = false;
	public hourError: boolean = false;
	public minuteError: boolean = false;

	public editing: boolean = false;
	public updating: boolean = false;

	@Prop({default: (): Date => new Date(1970, 1, 1, 10, 30)}) private value: Date;

	@Watch("day")
	@Watch("hour")
	@Watch("minute")
	public update(): void {
		if (this.updating) {
			return;
		}

		this.editing = true;
		this.dayError = (!this.day) || this.day.length == 0;
		this.hourError = this.hour < 0 || this.hour > 23;
		this.minuteError = this.minute < 0 || this.minute > 59;

		if (this.dayError || this.hourError || this.minuteError) {
			this.$nextTick(() => {
				this.$emit("input", null);
				this.editing = false;
			});
			return;
		}
		
		let baseDate = this.parseDateString(this.day);

		let d = new Date(baseDate.getFullYear(), baseDate.getMonth(), baseDate.getDate(), this.hour, this.minute);
		this.$nextTick(() => {
			this.$emit("input", d);
			this.editing = false;
		});
	}

	public parseDateString(date: string): Date {
		let bits = date.split("-");
		
		let dt = new Date(Number(bits[0]), Number(bits[1]) - 1, Number(bits[2]));
		return dt;
	}

	public createDateString(date: Date) : string {
		let year = date.getFullYear();
		let month = date.getMonth() + 1;
		let day = date.getDate();

		let dt = "" + year + "-" + (month + 1 < 10 ? "0" + month : month) + "-" + (day < 10 ? "0" + day : day);
		return dt;
	}

	@Watch("value", { immediate: true })
	private valueChanged(value: Date, old: Date) {
		if(this.editing) {
			return;
		}

		this.updating = true;
		
		this.$nextTick(() => {
			if (value) {
				this.day = this.createDateString(value);
				this.hour = value.getHours();
				this.minute = value.getMinutes();
			} else {
				this.day = '';
				this.hour = 0;
				this.minute = 0;
			}
			this.$nextTick(() => {
				this.updating = false;
			});
		});
	}

	public ValueChanged(date: Date) {
		this.valueChanged(date, null);
	}
}
</script>