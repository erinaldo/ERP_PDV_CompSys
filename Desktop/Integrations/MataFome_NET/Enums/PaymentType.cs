using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MataFome.DB.Enums
{
    public enum PaymentType
    {
        [Display(Name = "Dinheiro", Description = "Dinheiro na Entrega")]
        Cash = 0,
        [Display(Name = "Cartão de Débito", Description = "Cartão de Débito na Entrega")]
        DebitCard = 1,
        [Display(Name = "Cartão de Crédito", Description = "Cartão de Crédito na Entrega")]
        CreditCard = 2,
        [Display(Name = "Cartão de Débito Online", Description = "Cartão de Débito Online")]
        DebitCardOnline = 3,
        [Display(Name = "Cartão de Crédito Online", Description = "Cartão de Crédito Online")]
        CreditCardOnline = 4,
        [Display(Name = "Dinheiro e Cartão Débito", Description = "Dinheiro e Cartão Débito na Entrega")]
        CashDebitCard = 5,
        [Display(Name = "Dinheiro e Cartão Crédito", Description = "Dinheiro e Cartão Crédito na Entrega")]
        CashCreditCard = 6,
    }
}
