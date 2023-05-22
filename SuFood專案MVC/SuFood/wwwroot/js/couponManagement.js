const vm = new Vue({
    el: '.couponManagement',
    data: {
        pageName: '優惠券管理',
        keyword: '',
        editId: undefined,
        popupShowing: {
            showPopup: false
        },
        CreateOrEdit: '',
        coupons: [],
        editCouponList: {
            CouponId: this.editId,
            CouponName: '666',
            CouponDescription: '666',
            CouponMinusCost: '777',
            MinimumPurchasingAmount: '777',
            CouponStartDate: '',
            CouponEndDate: ''
        },
        manage: {
            CouponId: 0,
            CouponName: '',
            CouponDescription: '',
            CouponMinusCost: '',
            MinimumPurchasingAmount: '',
            CouponStartDate: '',
            CouponEndDate: ''
        }
    },
    methods: {
        toastHint() {
            this.toastHintStyle.fadeInUp = true;
            setTimeout(() => {
                this.toastHintStyle.fadeInUp = false
            }, 2100)
        },
        createCoupon() {
            this.CreateOrEdit = "Create";
            this.popupShowing.showPopup = true;
        },
        closeCoupon() {
            this.popupShowing.showPopup = false;
        },
        editCoupon(item) {
            this.CreateOrEdit = "Edit";
            this.popupShowing.showPopup = true;
            this.editCouponList = item;
            this.editId = item.couponId;
        },
        delCoupon(DelCouponItem) {
            // this.editCouponList = DelCouponItem;
        },
        GetCouponInfo() {
            let _this = this;
            couponInfo = axios('/BackStage/CouponManagement/GetCoupons').then(response => {
                _this.coupons = response.data
            });
        },
        CreateCoupon() {
            let _this = this;
            axios.post('/BackStage/CouponManagement/Create', this.manage, {
                headers: {
                    'Content-Type':'application/json'
                }
            }).then(response => {
                this.popupShowing.showPopup = false
                alert(response.data);
                _this.GetCouponInfo()
            })
        },
        EditCoupon(item) {
            let _this = this;
            var tempData = {
                CouponId: _this.editId,
                CouponName: _this.editCouponList.couponName,
                CouponDescription: _this.editCouponList.couponDescription,
                CouponMinusCost: _this.editCouponList.couponMinusCost,
                MinimumPurchasingAmount: _this.editCouponList.minimumPurchasingAmount,
                CouponStartDate: _this.editCouponList.couponstartdate2String,
                CouponEndDate: _this.editCouponList.couponenddate2String,
            };
            console.log(tempData);
            axios.put("/BackStage/CouponManagement/Edit/", tempData, {
                headers: {
                    'Content-Type':'application/json'
                }
            }).then(response => {
                alert(response.data)
                this.popupShowing.showPopup = false;
                _this.GetCouponInfo()
            })
        },
    },
    computed: {
        filterCoupons() {
            let keywordLowerCase = this.keyword.toLowerCase();
            return this.coupons.filter(c => {
                return c.couponName.toLowerCase().indexOf(keywordLowerCase) !== -1;
            })
        }
    },
    mounted() {
        this.GetCouponInfo();
    }
});