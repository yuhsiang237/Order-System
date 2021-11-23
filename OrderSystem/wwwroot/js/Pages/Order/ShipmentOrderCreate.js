(function () {
    var vm = new Vue({
        el: '#CreateForm',
        data: {
            Number: "",
            Type: "",
            DeliveryDate: "",
            FinishDate: "",
            Remarks: "",
            Address: "",
            SignName: "",
            Errors: {},
            ProductOption:[],
            OrderDetails: [
                {
                    ProductId:31,
                    ProductNumber: "C-1",
                    ProductUnit: 5,
                    ProductPrice: 8,
                    ProductName: "商品1",
                    ProductRemarks: "備註"
                },
                {
                    ProductId: 32,
                    ProductNumber: "C-1",
                    ProductUnit: 5,
                    ProductPrice: 10,
                    ProductName: "商品1",
                    ProductRemarks: "備註"
                }
            ]
        },
        mounted: function () {
            this.ProductOption = $Page.ProductData;
        },
        computed: {
            Total: function () {
                var _total = 0;
                this.OrderDetails.forEach(item => {
                    _total += Number(item.ProductUnit) * Number(item.ProductPrice);
                })
                return _total;
            }
        },
        methods: {
            productSelectChange(e) {
                console.log(e)
            },
            createRow() {
                this.OrderDetails.push({});
            },
            deleteRow(index) {
                this.OrderDetails.splice(index, 1);
            },
            createOrder() {
                console.log(this.DeliveryDate)
                $.ajax({
                    type: 'POST',
                    context: this,
                    url: '/Order/ShipmentOrderCreate',
                    contentType: 'application/x-www-form-urlencoded',
                    headers: {
                        "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val()
                    },
                    data: {
                        Order: {
                            Number: this.Number,
                            Type: this.Type,
                            DeliveryDate: this.DeliveryDate,
                            FinishDate: this.FinishDate,
                            Total: this.Total,
                            Remarks: this.Remarks,
                            Address: this.Address,
                            SignName: this.SignName
                        },
                        OrderDetails: this.OrderDetails
                    },
                    success: function (res) {
                        if (res.isSuccess) {
                            alert('成功建立訂單!')
                            this.Errors = {};
                        } else {
                            
                            this.Errors = res.error;
                        }
                    },
                    error: function () { alert('A error'); }
                })
            }
        }
    })
    $(document).ready(function () {
        $("#DeliveryDate").datepicker().on("change", function (e) {
            vm.$data.DeliveryDate = $(this).val();
        });
        $("#FinishDate").datepicker().on("change", function (e) {
            vm.$data.FinishDate = $(this).val();
        });
    })

})()
