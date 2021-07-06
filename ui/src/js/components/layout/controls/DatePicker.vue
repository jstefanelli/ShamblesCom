<template>
	<div class="flex align-center">
		<div class="flex flex-grow align-center">
			<span>Day:</span>
			<input type="date" :class="'margin-h-10 sm' + (dayError ? ' error' : '')" v-model="day" />
		</div>
		<div class="flex flex-grow-3 align-center">
			<span>Hour:</span>
			<input type="number" :class="'margin-h-10 sm ' + (hourError ? ' error' : '')" v-model.number="hour" />

			<span>Minute:</span>
			<input type="number" :class="'margin-l-10 sm ' + (minuteError ? ' error' : '')" v-model.number="minute" />
		</div>
	</div>
</template>

<script lang="ts">
import { Vue, Component, Prop, Watch } from 'vue-property-decorator';


@Component({
	components: {

	}
})
export default class extends Vue {
	private day: string = '';
	private hour: number = 0;
	private minute: number = 0;

	private dayError: boolean = false;
	private hourError: boolean = false;
	private minuteError: boolean = false;

	private editing: boolean = false;
	private updating: boolean = false;

	@Prop({default: (): Date => new Date(1970, 1, 1, 10, 30)}) private value: Date;

	@Watch("day")
	@Watch("hour")
	@Watch("minute")
	private update(): void {
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

	private parseDateString(date: string): Date {
		let bits = date.split("-");
		
		let dt = new Date(Number(bits[0]), Number(bits[1]), Number(bits[2]));
		return dt;
	}

	private createDateString(date: Date) : string {
		let year = date.getFullYear();
		let month = date.getMonth();
		let day = date.getDate();

		let dt = "" + year + "-" + (month < 10 ? "0" + month : month) + "-" + (day < 10 ? "0" + day : day);
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