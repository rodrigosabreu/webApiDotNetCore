using System;

namespace WebApi.Helpers
{
    public static class DateTimeExtensions
    {
        public static int ObterIdadeAtual(this DateTime dataNascimento)
        {
            var dataAtual = DateTime.UtcNow;
            int idade = dataAtual.Year - dataNascimento.Year;
            idade = (dataAtual.DayOfYear < dataNascimento.DayOfYear) ? (idade - 1) : idade;

            return idade;
        }
    }
}
