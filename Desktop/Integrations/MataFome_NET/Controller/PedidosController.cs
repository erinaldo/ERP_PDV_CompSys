

using MataFome.DB.Enums;
using MataFome_NET.Model;
using PDV.DAO.Entidades;
using MataFome.API.Client.Extensions;
using PDV.DAO.Entidades.PDV;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;

namespace MataFome_NET.Controller
{
    public class PedidosController
    {
        public static ContextMataFome contextMataFome;
        static PedidosController()
        {
            contextMataFome = new ContextMataFome();
        }

        public class Pedido
        {
            public long ID { get; set; }
            public DateTime Data { get; set; }
            public string Cliente { get; set; }
            public decimal Total { get; set; }
            public decimal Troco { get; set; }
            public string Telefone { get; set; }
            public string CPF { get; set; }
            public string Endereco { get; set; }
            public string Numero { get; set; }
            public string Bairro { get; set; }
            public PaymentType Pagamento { get; set; }
            public OrderStatus Status { get; set; }
            public string Observacao { get; set; }
            public string StatusPedido { get { return Status.GetDisplayName(); } }
            public string PgamentoPedido { get { return Pagamento.GetDisplayName(); } }
        }
        public static List<Pedido> ListaPedido()
        {
            try
            {
             var retorno= (from x in contextMataFome.Orders
                              select new Pedido
                              {
                                  ID = x.ID,
                                  Data = x.Created,
                                  Cliente = x.CustomerName,
                                  Total = x.OrderTotal,
                                  Troco = x.Change,
                                  Telefone = x.CustomerPhone,
                                  CPF = x.CustomerCPF,
                                  Endereco = x.AddressStreet,
                                  Numero = x.AddressNumber,
                                  Bairro = x.AddressDistrict,
                                  Pagamento = (PaymentType)x.PaymentType,
                                  Status = (OrderStatus)x.OrderStatus,
                                  Observacao = x.Observation
                              }).Where(x=>x.Status == OrderStatus.Delivered).ToList();

                return retorno;
            }
            catch (System.Exception)
            {
                throw;
            }
        }
        public static List<Orders> GetOrders()
        {
            try
            {
                var retorno = contextMataFome.Orders.Where(x => x.OrderStatus == (int)OrderStatus.Delivered).ToList();
                return retorno;
            }
            catch (System.Exception)
            {
                throw;
            }
        }
        public static List<OrderProducts> GetOrdersProduct()
        {
            try
            {
                string query = @"select
                                    op.ID,
                                    op.OrderID,
                                    op.ProductID,
                                    op.ExternalID,
                                    op.Name,
                                    op.Price,
                                    op.Discount,
                                    op.Amount,
                                    op.TotalPrice,
                                    op.Observation,
                                    op.AdditionalFillingsIDs,
                                    op.Additionals,
                                    op.AdditionalsPrice
                                    from  OrderProducts op
                                    join Orders o on o.ID = op.OrderID
                                    where =" + (int)OrderStatus.Delivered;
                var retorno = contextMataFome.Database.SqlQuery<OrderProducts>(query).ToList();
                return retorno;
            }
            catch (System.Exception)
            {
                throw;
            }
        }
        public static List<Orders> ListaPedidoPoID(int ID)
        {
            try
            {
                return contextMataFome.Orders.Where(x => x.ID == ID).ToList();

            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public static List<OrderProducts> ListaPedidoItem()
        {
            try
            {
                return contextMataFome.OrderProducts.ToList();

            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public static List<OrderProducts> ListaPedidoItemPorVenda(int ID)
        {
            try
            {
                return contextMataFome.OrderProducts.Where(x => x.ID == ID).ToList();

            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public static void AtualizarStatus(OrderStatus orderStatus, int ID)
        {
            try
            {
                Orders orders = contextMataFome.Orders.Where(x => x.ID == ID).FirstOrDefault();
                orders.OrderStatus = (int)orderStatus;
                contextMataFome.Orders.Add(orders);
                contextMataFome.Entry(orders).State = EntityState.Modified;
                contextMataFome.Entry(orders).Property(x => x.Created).IsModified = false;
                contextMataFome.SaveChangesAsync();
            }
            catch (System.Exception)
            {

                throw;
            }
        }


        public static void AtualizarPedido(Venda Venda)
        {
            try
            {

            }
            catch (System.Exception)
            {
                throw;
            }
        }


    }

}