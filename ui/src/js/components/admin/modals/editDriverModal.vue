<template>
	<modal ref="myModal">
		<template slot="header">
			{{ driver && driver.id != 0 ? "Edit Driver:" : "Add a new Driver:" }}
		</template>

		<template>
			<div class="flex flex-vertical">
				<span v-if="(!driver) || driver.id == 0" class="margin-b-10">
					WARNING: A new driver might not appear in the list until you add a race result for that driver
				</span>

				<span v-if="errors && errors.Nickname" class="color-red margin-b-10"><b>Error:</b> {{ errors.Nickname }}</span>
				<span>Nickname:</span>
				<input type="text" class="margin-v-10" v-model="name"/>

				<span v-if="errors && errors.DateTime" class="color-red margin-b-10"><b>Error:</b> {{ errors.DateTime }}</span>
				<span>Driver Number:</span>
				<input type="number" v-model.number="number" class="margin-v-10" />
			</div>
		</template>

		<template slot="footer">
			<button @click="close" class="passive">
				Cancel
			</button>
			<button @click="submit">
				Save
			</button>
		</template>
	</modal>
</template>

<script lang="ts">
import Modal from '@/components/modal/modal.vue';
import Driver from '@/data/Driver';
import Season from '@/data/Season';
import SPA from '@/SPA/spa';
import { Vue, Component, Prop, Watch } from 'vue-property-decorator';

@Component({
	components: {
		Modal,
	}
})
export default class extends Vue {
	@Prop({default: (): Season => null}) private season: Season;
	@Prop({default: (): Driver => null}) private driver: Driver;
	private errors: any = {};
	private name: string = "";
	private number: number = 0;

	private modal(): any {
		return (this.$refs.myModal as any);
	}

	@Watch("driver")
	private raceChanged(val: Driver, old: Driver) {
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
					this.errors = errors.errors;
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
					this.errors = errors.errors;
				} else {
					this.errors = {
						Name: "Server Error"
					};
				}
			});
		}
	}

	private close() {
		this.modal().closeModal();
	}

	public open() {
		this.modal().openModal();
	}
}
</script>