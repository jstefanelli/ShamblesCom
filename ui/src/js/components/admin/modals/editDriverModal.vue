<template>
	<modal ref="myModal">
		<template slot="header">
			{{ driver && driver.id != 0 ? "Edit Driver:" : "Add a new Driver:" }}
		</template>

		<template>
			<div class="flex flex-col">
				<error-display v-if="errors && errors.Nickname" :errors="errors.Nickname" />
				<span>Nickname:</span>
				<my-input type="text" class=" my-2" v-model="name"/>

				<error-display v-if="errors && errors.Number" :errors="errors.Number" />
				<span>Driver Number:</span>
				<my-input type="number" v-model.number="number" class="my-2" />
			</div>
		</template>

		<template slot="footer">
			<my-button @click="close">
				Cancel
			</my-button>
			<my-button @click="submit" :main="true">
				Save
			</my-button>
		</template>
	</modal>
</template>

<script lang="ts">
import ErrorDisplay from '@/components/common/ErrorDisplay.vue';
import Modal from '@/components/modal/modal.vue';
import Driver from '@/data/Driver';
import Season from '@/data/Season';
import SPA from '@/SPA/spa';
import MyInput from '@/components/layout/controls/myinput.vue';
import MyButton from '@/components/layout/controls/mybutton.vue';
import { Vue, Component, Prop, Watch } from 'vue-property-decorator';

@Component({
	components: {
		Modal,
ErrorDisplay,
MyInput,
MyButton
	}
})
export default class extends Vue {
	@Prop({default: (): Season => null}) public readonly season: Season;
	@Prop({default: (): Driver => null}) public readonly driver: Driver;
	public errors: any = {};
	public name: string = "";
	public number: number = 0;

	public modal(): any {
		return (this.$refs.myModal as any);
	}

	@Watch("driver")
	public driverChanged(val: Driver, old: Driver) {
		if (val) {
			this.name = val.nickname;
			this.number = val.number;
		} else {
			this.name = '';
			this.number = 0;
		}
	}

	public submit(): void {
		let d: Driver = {
			id: this.driver ? this.driver.id : 0,
			nickname: this.name,
			number: this.number
		};

		if (d.id == 0) {
			SPA.navigateAndRender("/admin/" + this.season.id + "/drivers", "POST", d, true).then(() => {
				this.errors = {};
			}).catch((errors) => {
				if (typeof(errors) == 'object') {
					if (typeof(errors.erroors) == 'object') {
						this.errors = errors.errors;
					} else {
						this.errors = errors;
					}
				}else {
					this.errors = {
						Name: "Server error"
					};
				}
			});
		} else {
			SPA.navigateAndRender("/admin/" + this.season.id + "/drivers/" + d.id, "PUT", d, true).then(() => {
				this.errors = {};
			}).catch((errors) => {
				if (typeof(errors) == 'object') {
					if (typeof(errors.erroors) == 'object') {
						this.errors = errors.errors;
					} else {
						this.errors = errors;
					}
				} else {
					this.errors = {
						Name: "Server Error"
					};
				}
			});
		}
	}

	public close() {
		this.modal().closeModal();
	}

	public open() {
		this.modal().openModal();
	}
}
</script>