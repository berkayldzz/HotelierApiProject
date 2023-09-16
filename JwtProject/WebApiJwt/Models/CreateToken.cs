using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace WebApiJwt.Models
{
    public class CreateToken
    {
        // Jwt oluşturma metodumuz
        public string TokenCreate()
        {
            var bytes = Encoding.UTF8.GetBytes("aspnetcoreapiapi");  // İlgili token key i geçiyorum. 

            SymmetricSecurityKey key = new SymmetricSecurityKey(bytes); // Gizli anahtar, SymmetricSecurityKey sınıfı kullanılarak oluşturuldu.

            // SigningCredentials , gizli anahtarı ve kullanılacak imza algoritmasını içeren kimlik bilgilerini temsil eder. 

            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // JWT token'ı oluşturulur. Bu token, başlık (header), yük (payload) ve imza (signature) içerir.

            JwtSecurityToken token = new JwtSecurityToken(issuer: "http://localhost", audience: "http://localhost", notBefore: DateTime.Now, expires: DateTime.Now.AddSeconds(20), signingCredentials: credentials);

            // JWT token'ını işlemek için bir JwtSecurityTokenHandler örneği oluşturulur.
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();  // Jwt oluşturucu
            return handler.WriteToken(token);
        }

        public string TokenCreateAdmin()
        {
            var bytes = Encoding.UTF8.GetBytes("aspnetcoreapiapi");

            SymmetricSecurityKey key = new SymmetricSecurityKey(bytes);

            SigningCredentials credentials = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

            // Claim oluşturarak rollerimizin içeriğini tutmasını sağlayacağız.

            List<Claim> claims = new List<Claim>()
            {
                // kullanıcının benzersiz bir id içerir. Guid.NewGuid().ToString() kullanılarak rasgele bir ID oluşturulur.
                new Claim(ClaimTypes.NameIdentifier,Guid.NewGuid().ToString()),

                // Bu satırlar, kullanıcının rollerini belirten talepleri ekler.
                new Claim(ClaimTypes.Role,"Admin"),
                new Claim(ClaimTypes.Role,"Visitor"),

            };

            JwtSecurityToken jwtSecurityToken=new JwtSecurityToken(issuer: "http://localhost", audience: "http://localhost", notBefore: DateTime.Now, expires: DateTime.Now.AddSeconds(30), signingCredentials: credentials,claims:claims);

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler(); 
            return handler.WriteToken(jwtSecurityToken);
        }
    }
}
