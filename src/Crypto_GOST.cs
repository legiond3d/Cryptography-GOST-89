using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_2
{
    /*!
        \brief Родительский класс для реализации шифра ГОСТ-89
    
        Хранит таблицу подстановки и реализует методы общие для алгоритмов шифрования и дешифрования (разбиение ключа и замещение байт)
    */
    public abstract class Crypto_GOST
    {
        //! Двумерный массив, хранящий таблицу подстановки для ГОСТ-89
        //таблица подстановки
        protected byte[][] sub_table =
        {
            new byte[] { 0xC, 0x4, 0x6, 0x2, 0xA, 0x5, 0xB, 0x9, 0xE, 0x8, 0xD, 0x7, 0x0, 0x3, 0xF, 0x1 },
            new byte[] { 0x6, 0x8, 0x2, 0x3, 0x9, 0xA, 0x5, 0xC, 0x1, 0xE, 0x4, 0x7, 0xB, 0xD, 0x0, 0xF },
            new byte[] { 0xB, 0x3, 0x5, 0x8, 0x2, 0xF, 0xA, 0xD, 0xE, 0x1, 0x7, 0x4, 0xC, 0x9, 0x6, 0x0 },
            new byte[] { 0xC, 0x8, 0x2, 0x1, 0xD, 0x4, 0xF, 0x6, 0x7, 0x0, 0xA, 0x5, 0x3, 0xE, 0x9, 0xB },
            new byte[] { 0x7, 0xF, 0x5, 0xA, 0x8, 0x1, 0x6, 0xD, 0x0, 0x9, 0x3, 0xE, 0xB, 0x4, 0x2, 0xC },
            new byte[] { 0x5, 0xD, 0xF, 0x6, 0x9, 0x2, 0xC, 0xA, 0xB, 0x7, 0x8, 0x1, 0x4, 0x3, 0xE, 0x0 },
            new byte[] { 0x8, 0xE, 0x2, 0x5, 0x6, 0x9, 0x1, 0xC, 0xF, 0x4, 0xB, 0x0, 0xD, 0xA, 0x3, 0x7 },
            new byte[] { 0x1, 0x7, 0xE, 0xD, 0x0, 0x5, 0x8, 0x3, 0x4, 0xF, 0xA, 0x6, 0x9, 0xC, 0xB, 0x2 }
        };

        /*!
            \brief Метод реализующий разбиение исходного ключа (256 бит) на 8 частей по 32 бита
            \param key - исходный ключ, который требуется разбить
            \return subkeys - массив из 8 вспомогательных ключей
        */
        protected uint[] generate_keys(byte[] key)//разбиваем исходный ключ (256 бит) на 8 ключей по 32 бита
        {
            uint[] subkeys = new uint[8];
            for (int i = 0; i < 8; i++)
            {
                subkeys[i] = BitConverter.ToUInt32(key, 4 * i);
            }
            return subkeys;
        }

        /*!
            \brief Метод осуществляющий замещение байта сообщения на байт из таблицы подстановки
            \param value - один байт сообщения
            \return output - изменённый байт сообщения
        */
        protected uint substitution(uint value)//непосредственно замещение байта сообщения на байт из таблицы подстановки
        {
            uint output = 0;
            byte tmp;
            for (int i = 0; i < 8; i++)
            {
                tmp = (byte)((value >> (4 * i)) & 0x0f);
                output = output | (UInt32)sub_table[i][tmp] << (4 * i);
            }
            return output;
        }
    }
}
