const vm = new Vue({
    el: ".Recent-order",
    data: {
        products: [],
        plans: [],
        selectedProducts: [],
        uplodaImgPreview: {
            preview: null,
            image: null,
        },
        EditProductsList: {
            planId: "",
            planName: "",
            planDescription: "",
            planTotalCount: "",
            planCanChoiceCount: "",
            planPrice: "",
        },
        EditOrDeleteorCreate: "Edit",
        modalContentStyle: {
            w200: true,
            w800: false
        },
        toastHintStlye: {
            fadeInUp: false
        },
        modalContainerStyle: {
            showModal: false,
        },

        //新方法
        SelectedItemStyle: {
            selected: false
        }
    },
    computed: {
        ModalTitle() {
            switch (this.EditOrDeleteorCreate) {
                case "Edit":
                    return "修改"
                    break;
                case "Create":
                    return "新增"
                    break;
                case "Delete":
                    return "刪除"
                    break;
            }
        },
    },
    methods: {
        previewImage: function (event) {
            console.log("change")
            var input = event.target;
            if (input.files) {
                var reader = new FileReader();
                reader.onload = (e) => {
                    this.uplodaImgPreview.preview = e.target.result;
                }
                this.uplodaImgPreview.image = input.files[0];
                reader.readAsDataURL(input.files[0]);
            }
        },
        EditProducts(item) {
            this.GetProducts(item.planId)
            this.GetAllProduct()
            this.EditProductsList.planId = item.planId
            this.EditProductsList.planName = item.planName
            this.EditProductsList.planDescription = item.planDescription
            this.EditProductsList.planTotalCount = item.planTotalCount
            this.EditProductsList.planCanChoiceCount = item.planCanChoiceCount
            this.EditProductsList.planPrice = item.planPrice
            this.uplodaImgPreview.preview = item.img
            this.EditOrDeleteorCreate = "Edit"
            this.modalContentStyle.w200 = false
            this.modalContentStyle.w800 = true
            this.openModal()
        },
        DeleteProducts() {
            this.EditOrDeleteorCreate = "Delete"
            this.modalContentStyle.w200 = true
            this.modalContentStyle.w800 = false
            this.openModal()
        },
        CreateProducts() {
            this.selectedProducts = []
            this.GetAllProduct()
            this.EditOrDeleteorCreate = "Create"
            this.modalContentStyle.w200 = false
            this.modalContentStyle.w800 = true
            this.uplodaImgPreview.preview = null
            this.openModal()
        },
        toastHint() {
            this.toastHintStlye.fadeInUp = true
            setTimeout(() => {
                this.toastHintStlye.fadeInUp = false
            }, 2100)

            this.EditProductsList.planId = ""
            this.EditProductsList.planName = ""
            this.EditProductsList.planDescription = ""
            this.EditProductsList.planTotalCount = ""
            this.EditProductsList.planCanChoiceCount = ""
            this.EditProductsList.planPrice = ""
        },
        openModal() {
            this.modalContainerStyle.showModal = true;
        },
        closeModal() {
            this.modalContainerStyle.showModal = false;
        },
        closeModalWithHint() {
            this.modalContainerStyle.showModal = false;
            this.toastHint();
        },
        //將所有商品放入選擇商品
        toggleProductSelection(product) {
            let productId = product.productId
            let spi = this.selectedProducts.map(p => p.productId)
            const index = spi.indexOf(productId);
            if (index !== -1) {
                this.selectedProducts.splice(index, 1);
            } else {
                this.selectedProducts.push(product);
            }
        },
        //方案選擇商品顏色提示
        isSelected(product) {
            let productId = product.productId
            let spi = this.selectedProducts.map(p => p.productId)
            return spi.includes(productId);
        },
        //查詢方案
        GetPlans() {
            axios.get("/BackStage/FreeChoicePlansManagement/GetPlansAndProducts").then(response => {
                response.data.forEach(response => {
                    console.log(response)
                    this.plans.push(response.plans)
                })
            })
        },
        //查詢方案內的產品
        GetProducts(planId) {
            let _this = this;
            axios.get("/BackStage/FreeChoicePlansManagement/GetPlansAndProducts").then(response => {
                response.data.forEach(response => {
                    if (response.plans.planId == planId) {
                        _this.selectedProducts = response.products
                    }
                })
            })
        },
        //查詢所有產品
        GetAllProduct() {
            let _this = this;
            axios.get("/BackStage/FreeChoicePlansManagement/GetAllProducts").then(response => {
                this.products = response.data
            })
        },
    },
    mounted() {
        this.GetPlans();
    }
})