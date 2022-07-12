<template>
    <input :type="type" :value="value" @input="$emit('input', $event.target.value)" @change="$emit('change')" :class="{
        'bg-black border-2 border-blac border-opacity-50 p-3 text-xl text-white rounded-md hover:border-white focus:border-opacity-100 focus:border-white active:border-opacity-100 active:border-white': (type != 'select' && type != 'submit' && type != 'file'),
        'bg-red-titlebar cursor-pointer p-3 rounded-md text-white border-0 border-red-titlebar disabled:bg-bg disabled:border-2 disabled:border-red-titlebar disabled:text-gray-200': type == 'submit',
        'bg-black text-white border-2 border-black border-opacity-50 rounded-md p-3 appearance-none': type == 'select'
    }" :disabled="disabled">
</template>

<script lang="ts">

import { Vue, Component, Prop } from 'vue-property-decorator';

@Component({
    components: {

    }
})
export default class extends Vue {
    @Prop({default: undefined}) public value: any;
    @Prop({default: "text"}) public type: string;
    @Prop({default: false}) public disabled: boolean;
}

</script>