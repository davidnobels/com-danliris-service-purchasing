﻿using Com.DanLiris.Service.Purchasing.Lib.Facades.GarmentInternNoteFacades;
using Com.DanLiris.Service.Purchasing.Lib.Models.GarmentDeliveryOrderModel;
using Com.DanLiris.Service.Purchasing.Lib.Models.GarmentInternNoteModel;
using Com.DanLiris.Service.Purchasing.Lib.Models.GarmentInvoiceModel;
using Com.DanLiris.Service.Purchasing.Test.DataUtils.GarmentInvoiceDataUtils;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.DanLiris.Service.Purchasing.Test.DataUtils.GarmentInternNoteDataUtils
{
    public class GarmentInternNoteDataUtil
    {
        private readonly GarmentInvoiceDataUtil garmentInvoiceDataUtil;
        private readonly GarmentInternNoteFacades facade;
        public GarmentInternNoteDataUtil(GarmentInvoiceDataUtil garmentInvoiceDataUtil, GarmentInternNoteFacades facade)
        {
            this.garmentInvoiceDataUtil = garmentInvoiceDataUtil;
            this.facade = facade;
        }

        public GarmentInternNote GetNewData()
        {
            var garmentInvoice = Task.Run(() => garmentInvoiceDataUtil.GetTestDataViewModel("User")).Result;
            List<GarmentInternNoteDetail> garmentInternNoteDetails = new List<GarmentInternNoteDetail>();
            foreach (var item in garmentInvoice.Items)
            {
                foreach (var detail in item.Details)
                {
                    garmentInternNoteDetails.Add(new GarmentInternNoteDetail
                    {
                        EPOId = detail.EPOId,
                        EPONo = detail.EPONo,
                        POSerialNumber = detail.POSerialNumber,
                        Quantity = (long)detail.DOQuantity,
                        RONo = detail.RONo,

                        DOId = item.DeliveryOrderId,
                        DONo = item.DeliveryOrderNo,
                        DODate = item.DODate,

                        ProductId = detail.ProductId,
                        ProductCode = detail.ProductCode,
                        ProductName = detail.ProductName,

                        PaymentType = item.PaymentType,
                        PaymentMethod = item.PaymentMethod,

                        PaymentDueDate = item.DODate.AddDays(detail.PaymentDueDays) ,

                        UOMId = detail.UomId,
                        UOMUnit = detail.UomUnit,

                        PricePerDealUnit = detail.PricePerDealUnit,
                        PriceTotal = detail.PricePerDealUnit * detail.PricePerDealUnit,
                    });
                }
            }



            List<GarmentInternNoteItem> garmentInternNoteItems = new List<GarmentInternNoteItem>
            {
                new GarmentInternNoteItem
                {
                    InvoiceId = garmentInvoice.Id,
                    InvoiceNo = garmentInvoice.InvoiceNo,
                    InvoiceDate = garmentInvoice.InvoiceDate,
                    TotalAmount = 20000,
                    Details = garmentInternNoteDetails
                }
            };

            GarmentInternNote garmentInternNote = new GarmentInternNote
            {
                INNo = "NI1234L",
                INDate = new DateTimeOffset(),

                SupplierId = garmentInvoice.SupplierId,
                SupplierCode = garmentInvoice.SupplierCode,
                SupplierName = garmentInvoice.SupplierCode,

                CurrencyId = garmentInvoice.CurrencyId,
                CurrencyCode = garmentInvoice.CurrencyCode,
                CurrencyRate = 5,
                

                Remark = null,

                Items = garmentInternNoteItems
            };
            return garmentInternNote;
        }

        public async Task<GarmentInternNoteItem> GetNewDataItem(string user)
        {
            var garmentInvoice = Task.Run(() => garmentInvoiceDataUtil.GetTestDataViewModel("User")).Result;
            return new GarmentInternNoteItem
            {

                InvoiceId = garmentInvoice.Id,
                InvoiceDate = garmentInvoice.InvoiceDate,
                InvoiceNo = garmentInvoice.InvoiceNo,
                TotalAmount = 2000,
                Details = new List<GarmentInternNoteDetail>
                            {
                                new GarmentInternNoteDetail
                                {
                                    EPOId=It.IsAny<int>(),
                                    EPONo="epono",
                                    DOId=It.IsAny<int>(),
                                    DODate = DateTimeOffset.Now,
                                    PaymentType="PaymentType",
                                    PaymentMethod = "PaymentMethod",
                                    UnitId = "UnitId",
                                    UnitName = "UnitName",
                                    UnitCode = "UnitCode",
                                    DONo="prno",
                                    RONo="12343",
                                    ProductId= It.IsAny<int>(),
                                    ProductCode="code",
                                    ProductName="name",
                                    UOMId=It.IsAny<int>(),
                                    UOMUnit="ROLL",
                                    Quantity=40,
                                    PricePerDealUnit=5000,
                                    POSerialNumber="PM132434",
                                    PaymentDueDays=2
                                }
                }
            };
        }

        public async Task<GarmentInternNote> GetTestData()
        {
            var data = GetNewData();
            await facade.Create(data,false, "Unit Test");
            return data;
        }
        
    }
}
