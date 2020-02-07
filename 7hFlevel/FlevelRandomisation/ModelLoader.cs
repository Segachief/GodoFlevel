using _7hFlevel.Helper;
using _7hFlevel.Indexing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _7hFlevel.FlevelRandomisation
{
    public class ModelLoader
    {
        public static byte[] SwapFieldModels(byte[] data)
        {

            int r = 0; // Iteration through model entries
            int o = 0; // Array index
            int c = 0; // Iteration through model's anims

            // Model Loader header
            // 0x00: Always 0
            // 0x02: Model Count
            // 0x04: Model Scale (unused)
            // 0x06: Model Loader Data starts

            try
            {
                // Get the number of models in this field
                byte[] modelCountByte = new byte[2];
                modelCountByte[0] = data[o + 2];
                modelCountByte[1] = data[o + 3];
                int modelCount = EndianMethods.GetLittleEndianIntTwofer(modelCountByte, 0);
                o += 6; // Skip data position past the header

                while (r < modelCount)
                {
                    // Take size of model name and convert it into an int for array index
                    byte[] modelNameSizeByte = new byte[2];
                    modelNameSizeByte[0] = data[o];
                    modelNameSizeByte[1] = data[o + 1];
                    int modelNameSize = EndianMethods.GetLittleEndianIntTwofer(modelNameSizeByte, 0);
                    o += 2;

                    // Skip 2-byte unknown value
                    o += 2;

                    // Jump past model name string to HRC location
                    o += modelNameSize;

                    // Read the current .HRC ID
                    byte[] oldHRCBytes = new byte[4];
                    oldHRCBytes[0] = data[o];
                    oldHRCBytes[1] = data[o + 1];
                    oldHRCBytes[2] = data[o + 2];
                    oldHRCBytes[3] = data[o + 3];

                    // ASCII conversion - string from .HRC bytes - Output: "AAAA"
                    string asciiString = @"" + Encoding.ASCII.GetString(oldHRCBytes, 0, oldHRCBytes.Length) + @"";

                    // Changes the HRC string
                    string newHRC = FieldModels.RandomModelSwap(asciiString);

                    // Converts the returned string into bytes
                    byte[] newHRCBytes = ConvertString.GetNameBytes(newHRC);

                    // Writes the new bytes to the .HRC
                    data[o] = newHRCBytes[0];
                    data[o + 1] = newHRCBytes[1];
                    data[o + 2] = newHRCBytes[2];
                    data[o + 3] = newHRCBytes[3];
                    o += 8; // Skip the .HRC part of the string

                    // Model Scale - Definitely have this as an option
                    // This is actually a string; '512' but written in ascii. Bear that in mind.
                    o += 4;

                    // Count the number of anims for this model
                    byte[] animCountByte = new byte[2];
                    animCountByte[0] = data[o];
                    animCountByte[1] = data[o + 1];
                    int animCount = EndianMethods.GetLittleEndianIntTwofer(animCountByte, 0);
                    o += 2;

                    // Light/Shading data - Will probably not be modifying these values
                    o += 30;

                    // Anims - Each anim has the following:
                    // 0x00: Size of anim name string
                    // 0x02: Anim name string
                    // 0x02 + Size: Unknown, 2-byte value
                    while(c < animCount)
                    {
                        // Take size of anim name and convert it into an int for array index
                        byte[] animNameSizeByte = new byte[2];
                        animNameSizeByte[0] = data[o];
                        animNameSizeByte[1] = data[o + 1];
                        int animNameSize = EndianMethods.GetLittleEndianIntTwofer(animNameSizeByte, 0);
                        o += 2;

                        // Changes the Anim string - Matches it to current HRC's anim pool
                        string newAnim = FieldModels.MatchedAnimSwap(asciiString, newHRC);

                        // Converts the string into bytes
                        byte[] newAnimBytes = ConvertString.GetNameBytes(newAnim);

                        // Writes the new bytes
                        data[o] = newAnimBytes[0];
                        data[o + 1] = newAnimBytes[1];
                        data[o + 2] = newAnimBytes[2];
                        data[o + 3] = newAnimBytes[3];
                        o += animNameSize; // Move past the anim name

                        // Unknown 2 bytes, skipped
                        o += 2;

                        c++;
                    }
                    c = 0;
                    r++;
                }
            }
            catch
            {
                MessageBox.Show("Flevel Chunk #3 (Model Loader) has encountered an issue; skipping current field");
            }
            return data;
        }
    }
}

//1
//aaaa
//afie
//ajif
//akee
//alad
//algd
//anic
//arfd
//asbf
//bgdc
//cbfe
//ccbc
//cdja
//cefd
//cfbb
//cfha
//cgda
//cgif
//ched
//ciac
//cige
//cjcc
//cjif
//ckfc
//clbb
//clgd
//cmde
//cmif
//coad
//cogb
//cpca
//crca
//crid
//csed
//ctcc
//ctib
//cufc
//cvba
//cvge
//cwed
//cyae
//cyif
//awcb
//awhf
//axdc
//azhe
//badd
//bbge
//bccf
//bcgd
//bfca
//bfhe
//bhff
//bijd
//bjfb
//bkbf
//blde
//bljc
//bmee
//bnaf
//bngd
//bocc
//bohe
//bpdc
//bpjd
//bqfb
//braf
//bsfc
//btec
//buge
//bvda
//bwab
//bwfd
//bxbe
//dafb
//dcic
//ddha
//deda
//dfgd
//dgcd
//dlfb
//dmcb
//dmia
//dndf
//dnje
//doib
//dpef
//dqae
//dqgd
//drcc
//dsbc
//dsgf
//dtce
//dtjb
//dufa
//dvbe
//dvhf
//dxbd
//dxje
//dyfd
//dzbb
//dzgf
//ebec
//ecbf
//ecib
//edjd
//eegc
//ehbe
//ehhc
//eiac
//eihd
//ejdc
//ekbf
//ekjb
//elgc
//emdf
//enab
//eseb
//etfe
//euaf
//euhb
//evfe
//ewbd
//ewje
//fbba
//fcaf
//fcgd
//fkdf
//flac
//flge
//fmcc
//fmib
//fnef
//fobe
//fpcb


//2
//anbd
//aodd
//atfe
//auff
//azbb
//bbab
//bgjc
//bkhd
//buac
//cahc
//cpjf
//cqga
//dbec
//dhaf
//diff
//edea
//eoea


//3
//abda
//acgd
//aiba
//aihb
//ayfb
//fqcb


//U
//aagb
//abjb
//adda
//aebc
//aehd
//afec
//aggb
//ahdf
//amcc
//aqgc
//asjc
//axja
//bdcd
//bdga
//beec
//bidb
//brgd
//brib
//bzda
//bzhf
//cnfb
//cnhf
//deie
//djid
//eaid
//epfb
//eqib
//exga
//eyie
//ezcc
//fbge
//feea
//ffha
//fgae
//fhaa
//fiba
//fjcf
//fzcc
//gdic
//gebb
//gehd
//gfdf
//ghgf
//gljd
//gmha



//O
//ateb
//auda
//aude
//avfe
//avhe
//awae
//awbe
//bxjb
//byba
//bybf
//bycd
//bydd
//bygf
//byib
//cade
//ccha
//cned
//ctbe
//czed
//czgb
//czgf
//dabf
//dcce
//dcfb
//dhge
//dhhf
//dhid
//djfa
//djfe
//dkie
//dkjd
//doga
//dria
//drif
//drje
//dtic
//eaga
//eagf
//ebca
//ebjf
//ecae
//echd
//eefb
//effb
//eghe
//eoac
//eoce
//faae
//fabb
//fabe
//facc
//fadc
//ffec
//feea
//fgec
//fgfb
//fghf
//fhic
//fhjb
//fjaf
//fjbd
//fkca
//fndf
//fqab
//fqbb
//fsdd
//gcjc
//ggef
//ggfe
//gghe
//ggid
//ggjc
//gjcf
//gjeb




//fqjb
//frgd
//fsge
//ftcf
//ftic
//fufe
//fved
//fwae
//fwgf
//fxjc

//gabe
//gajc
//gbia
//gcbd
//gchc
//ghad
//giha
//gjab

//gjha
//gkcf
//gkid
//gleb · 21 bones
//kaku4_sk
//Mog
// · 20 bones
//mogrif_sk
//Pink Mog
// · 20 bones
//mogrim_sk
//Bright Mog
//gnca · 20 bones
//mogriw_sk
//Yellow Mog
//gngb · 20 bones
//mogriy_sk
//Pink Mog
//goac · 20 bones
//mogrip_sk
//Choco Square Teller
//gofd · 21 bones
//cgirl_st
//Joe
//gpcd · 21 bones
//race1_sk
//Blue Jockey
//gpjb · 21 bones
//race2_sk
//Green Jockey
//gqfe · 21 bones
//race3_sk
//Choco Elevator(From Corel Prison)
//grcc · 0 bones
//gldelev_sk
//Cosmo Canyon Greeter
//grga · 21 bones
//cm_sk
//Cosmo Kid
//gsbe · 21 bones
//cg_sk
//Cosmo Propellor
//gshc · 0 bones
//cos_pro1_sk
//Cosmo Mother
//gsje · 21 bones
//cw_sk
//Cosmo Elderly Lady
//gtfc · 21 bones
//cow_sk
//Cosmo Kid Boy
//guba · 21 bones
//cb_sk
//Cosmo Door
//guhc · 0 bones
//cos_dor1_sk
//Cosmo Door
//guib · 0 bones
//cos_dor2_sk
//Glacier Map
//gujc · 0 bones
//icemap_sk
//Radar Dish
//gvae · 0 bones
//cos_ant_sk
//Weather Vane
//gvbc · 0 bones
//cos_tori_sk
//Yellow Huge Materia
//gvce · 0 bones
//4hmaty_sk
//Green Huge Materia
//gvdc · 0 bones
//4hmatg_sk
//Red Huge Materia
//gvea · 0 bones
//4hmatr_sk
//Blue Huge Materia
//gvee · 0 bones
//4hmatb_sk
//Cosmo Observatory Planet/Two Moons
//gwaa · 2 bones
//planet_sk
//Bugenhagen Lying down(Green orb missing)
//gwcc · 18 bones
//bu2_sk
//Green orb
//gwib · 0 bones
//sparkle_sk
//Rocket Town Citizen
//gwif · 21 bones
//man2_sk
//Rocket Town Bored Citizen gxgc
//gxef · 21 bones
//man3_sk
//Rocket Town Citizen
//gydc · 21 bones
//rkt_w_sk
//Young Cid
//gzad · 21 bones
//ycid_sk
//Door Base; Rocket
//gzgb · 0 bones
//rkindr_l_sk
//Door Base; Rocket
//gzha · 0 bones
//rkindr_r_sk
//Rocket Technician
//gzhf · 21 bones
//mac_sk
//Debris, pins Cid
//hcef · 0 bones
//hahen_sk
//Wutai Citizen male
//hdbb · 21 bones
//utma3_sk
//Wutai Citizen, Staniv hdic
//hdgf · 21 bones
//utman_sk
//Wutai Citizen, Chekhov
//hecd · 21 bones
//utwom_sk
//Wutai Citizen female
//heib · 21 bones
//utwo2_sk
//Wutai Citizen, Gorki
//hffb · 21 bones
//utma2_sk
//Wutai Citizen, Shake
//hgaf · 21 bones
//utchi_sk
//Yuffie House Cage
//hgia · 0 bones
//ori_sk
//Godo
//hgjd · 21 bones
//godoo_sk
//Wutai Door
//hhge · 0 bones
//husuma1l_sk
//Wutai Door
//hhhd · 0 bones
//husuma1r_sk
//Wutai Panel
//hhic · 0 bones
//rolldr_sk
//Corneo Ninja
//hhjf · 21 bones
//shinobi_sk
//Component, Gong-mechanism possibly
//hjdc · 0 bones
//kuromtra_sk
//Ancient Temple Chest
//hjga · 2 bones
//trbox_k_sk
//Rolling Stone
//hjhf · 0 bones
//rollobj_sk
//Clock, Minute/Sec Hand
//hjie · 0 bones
//line_a_sk
//Clock, Minute/sec Hand
//hjjd · 0 bones
//line_b_sk
//Clock, Hour Hand
//hkac · 0 bones
//line_c_sk
//Clock core, base
//hkbb · 0 bones
//kabe_sk
//Clock Core, mouth
//hkea · 2 bones
//kao_sk
//Temple Puzzle
//hkhb · 0 bones
//jtmpobj_sk
//Demon Wall
//hkjc · 26 bones
//demon_sk
//Green Digger
//hlfc · 21 bones
//man_st
//Purple Digger
//hmbe · 21 bones
//woman_sk
//Ancient Forest, Fly
//hmif · 5 bones
//batta_sk
//Ancient FOrest, Frog
//hnaf · 13 bones
//frog_sk
//Ancient Forest, Tongue Feeler
//hneb · 13 bones
//bero_sk
//Ancient Forest, Bee Hive
//hnif · 4 bones
//hana_k_sk
//Ancient Key
//hobd · 0 bones
//coralkey_sk
//Icicle Kid Girl
//hpce · 21 bones
//snchi_sk
//Icicle Man
//hpib · 21 bones
//snman_sk
//Blue Panel
//hqgc · 0 bones
//sori_sk
//Snowboard
//hqhc · 0 bones
//board_sk
//Red Flag
//hrae · 6 bones
//triflag_sk
//Blue Chest
//hrce · 2 bones
//trb_metb_sk
//Tumbling Rock
//hree · 0 bones
//rock_sk
//Ice Stalagmite
//hrff · 0 bones
//toge1_sk
//Ice Stalagmite
//hrha · 0 bones
//toge2_sk
//Ice Stalagmite
//hrhe · 0 bones
//toge3_sk
//Ice Stalagmite
//hseb · 0 bones
//icecle_sk
//Ultimate Weapon
//hsjd · 29 bones
//ultima_sk
//Handicapable Cloud
//htje · 29 bones
//chair_sk
//Train Cars, Corel Chase
//hvcf · 0 bones
//kisya_m_sk
//Red XIII Para Freefall
//hvjf · 29 bones
//P_red_sk
//Parachute open texture
//hwib · 0 bones
//para_sk
//Proud Clod
//hxbc · 27 bones
//robo_sk
//Jenova Synthesis
//hyfd · 29 bones
//jenova_sk
//Yellow Projectile(Diamond Wep Attacks)
//iajd · 0 bones
//beam_sk
//Zack, no sword
//ibad · 21 bones
//lzacks_sk
//Zack, w/sword
//ibgd · 21 bones
//szacks_sk
//mmmo · 23 bones
//sd_leno_sk
