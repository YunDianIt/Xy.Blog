<template>
  <BasicDrawer
    v-bind="$attrs"
    @register="registerDrawer"
    showFooter
    :title="getTitle"
    width="500px"
    @ok="handleSubmit"
  >
    <BasicForm @register="registerForm" />
  </BasicDrawer>
</template>
<script lang="ts">
  import { defineComponent, ref, computed, unref } from 'vue'
  import { BasicForm, useForm } from '/@/components/Form/index'
  import { BasicDrawer, useDrawerInner } from '/@/components/Drawer'

  import { formSchema } from './role.data'
  import { AddRole, UpdateRole } from '/@/api/sys/role'

  export default defineComponent({
    name: 'RoleDrawer',
    components: { BasicDrawer, BasicForm },
    emits: ['success', 'register'],
    setup(_, { emit }) {
      const isUpdate = ref(true)
      let rowId: number

      const [registerForm, { resetFields, setFieldsValue, validate }] = useForm({
        labelWidth: 90,
        schemas: formSchema,
        showActionButtonGroup: false,
      })

      const [registerDrawer, { setDrawerProps, closeDrawer }] = useDrawerInner(async (data) => {
        resetFields()
        setDrawerProps({ confirmLoading: false })
        isUpdate.value = !!data?.isUpdate
        console.log(data)
        if (unref(isUpdate)) {
          rowId = data.record.id
          console.log(data.record)
          setFieldsValue({
            ...data.record,
          })
        }
      })

      const getTitle = computed(() => (!unref(isUpdate) ? '新增角色' : '编辑角色'))

      async function handleSubmit() {
        try {
          const values = await validate()
          setDrawerProps({ confirmLoading: true })

          if (unref(isUpdate)) {
            values.id = rowId
            await UpdateRole(values)
          } else {
            await AddRole(values)
          }

          closeDrawer()
          emit('success', {
            isUpdate: unref(isUpdate),
            values: { ...values, id: rowId },
          })
        } finally {
          setDrawerProps({ confirmLoading: false })
        }
      }

      return {
        registerDrawer,
        registerForm,
        getTitle,
        handleSubmit,
      }
    },
  })
</script>
