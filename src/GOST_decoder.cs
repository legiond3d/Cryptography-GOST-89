using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_2
{
    /*!
        \brief Дочерний класс, реализующий дешифрование алгоритмом ГОСТ-89
    
        Реализует методы для дешифрования всего сообщения и одного блока сообщения
    */
    public class GOST_decoder : Crypto_GOST
    {
        /*!
            \brief Метод реализующий дешифрование всего сообщения
            \param Data - строка с зашифрованным сообщением
            \param key - строка с исходным ключом
            \return res - массив байт с расшифрованным сообщением
        */
        public byte[] decode(string Data, string Key)
        {
            byte[] data = Encoding.Unicode.GetBytes(Data);//преобразуем строку с данными для расшифрования в массив байтов
            byte[] key = Encoding.Unicode.GetBytes(Key);//преобразуем строку с ключом в массив байтов
            uint[] subkeys = generate_keys(key);//разбиваем ключ на 8 частей
            byte[] res = new byte[data.Length];//массив для результата дешифрования
            byte[] block = new byte[8];//временный массив для хранения одного блока данных (8 байт)
            for (int i = 0; i < data.Length / 8; i++)//цикл по всем восьмибайтовым блокам 
            {
                Array.Copy(data, 8 * i, block, 0, 8);//выбираем текущий блок
                Array.Copy(decode_block(block, subkeys), 0, res, 8 * i, 8);//шифруем выбранный блок
            }
            return res;
        }

        /*!
            Метод реализующий дешифрование одного блока (8 байт) сообщения
            \param block - байтовый массив, хранящий текущий блок
            \param subkeys - массив с вспомогательными ключами (части исходного ключа)
            \warning Блок сообщения должен быть полным
            \return res - расшифрованный блок сообщения
        */
        private byte[] decode_block(byte[] block, uint[] subkeys)//функция расшифровки одного блока (8 байт)
        {
            //исходный блок делится пополам на блоки N1 и N2
            uint N1 = BitConverter.ToUInt32(block, 0);
            uint N2 = BitConverter.ToUInt32(block, 4);
            for (uint i = 0; i < 32; i++)
            {
                uint index = i < 8 ? (i % 8) : (7 - i % 8);//первые 8 бит чередуется от 0 до 7, последующие биты чередуются от 7 до 0
                uint s = (N1 + subkeys[index]) % uint.MaxValue;
                s = substitution(s);//выполняем подстановку
                s = (s << 11) | (s >> 21);//циклический сдвиг влево на 11 разрядов 32 битного числа
                s = s ^ N2;//сложение по модулю 2
                if (i < 31)
                {
                    N2 = N1;
                    N1 = s;
                }
                else//на последней итерации цикла N1 не изменяется
                {
                    N2 = s;
                }
            }
            byte[] res = new byte[8];
            byte[] N1b = BitConverter.GetBytes(N1);
            byte[] N2b = BitConverter.GetBytes(N2);
            for (int i = 0; i < 4; i++)//совмещаем блоки N1 и N2
            {
                res[i] = N1b[i];
                res[4 + i] = N2b[i];
            }
            return res;
        }
    }
}
