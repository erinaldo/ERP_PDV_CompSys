using System;
using System.Collections.Generic;
using System.Text;

namespace MataFome.DB.Enums
{
    public enum OrderStatus
    {
        /// <summary>
        /// Aberto/Salvo, antes de finalizar
        /// </summary>
        Open = 0,
        /// <summary>
        /// Fechando - Usuario finalizou o pedido... 
        /// </summary>
        Closed = 1,
        /// <summary>
        /// Aguardando Aprovação - Mesmo que o Closed, faz mais sentido quando tiver cartão de crédito
        /// </summary>
        AwaitingApproval = 2,
        /// <summary>
        /// Aprovado - Nesse momento acredita-se que esta na cozinha pra ser preparado
        /// </summary>
        Approved = 3,
        /// <summary>
        /// Aguardando entrega - esperando o cliente pegar ou esperando motoboy
        /// </summary>
        AwaitingDelivery = 4,
        /// <summary>
        /// Em entrega - Com o motoboy
        /// </summary>
        Delivering = 5,
        /// <summary>
        /// Entregue - Talvez tenha um processo pós compra que o restaurante queira fazer usa esse estado antes de finalizar
        /// </summary>
        Delivered = 6,
        /// <summary>
        /// Finalizado 
        /// </summary>
        Finalized = 7,
        /// <summary>
        /// Cancelado
        /// </summary>
        Canceled = 8,
    }
}

