//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http;
//using System.Text;
//using System.Web.Http;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using mt_web_api;
//using mt_web_api.Controllers;
//
//namespace mt_web_api.Tests.Controllers {
//    [TestClass]
//    public class ValuesControllerTest {
//        [TestMethod]
//        public void Get() {
//            // Arrange
//            DbImportsController controller = new DbImportsController();
//
//            // Act
//            IEnumerable<string> result = controller.Get();
//
//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual(2, result.Count());
//            Assert.AreEqual("value1", result.ElementAt(0));
//            Assert.AreEqual("value2", result.ElementAt(1));
//        }
//
//        [TestMethod]
//        public void GetById() {
//            // Arrange
//            DbImportsController controller = new DbImportsController();
//
//            // Act
//            string result = controller.Get(5);
//
//            // Assert
//            Assert.AreEqual("value", result);
//        }
//
//        [TestMethod]
//        public void Post() {
//            // Arrange
//            DbImportsController controller = new DbImportsController();
//
//            // Act
//            controller.Post("value");
//
//            // Assert
//        }
//
//        [TestMethod]
//        public void Put() {
//            // Arrange
//            DbImportsController controller = new DbImportsController();
//
//            // Act
//            controller.Put(5, "value");
//
//            // Assert
//        }
//
//        [TestMethod]
//        public void Delete() {
//            // Arrange
//            DbImportsController controller = new DbImportsController();
//
//            // Act
//            controller.Delete(5);
//
//            // Assert
//        }
//    }
//}
