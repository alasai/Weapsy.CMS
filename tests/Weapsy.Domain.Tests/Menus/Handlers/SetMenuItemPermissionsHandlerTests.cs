﻿using System;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Model.Menus;
using Weapsy.Domain.Model.Menus.Commands;
using Weapsy.Domain.Model.Menus.Handlers;
using Weapsy.Tests.Factories;

namespace Weapsy.Domain.Tests.Menus.Handlers
{
    [TestFixture]
    public class SetMenuItemPermissionsHandlerTests
    {
        [Test]
        public void Should_throw_exception_when_menu_is_not_found()
        {
            var command = new SetMenuItemPermissions
            {
                SiteId = Guid.NewGuid(),
                MenuId = Guid.NewGuid()
            };

            var repositoryMock = new Mock<IMenuRepository>();
            repositoryMock.Setup(x => x.GetById(command.SiteId, command.MenuId)).Returns((Menu)null);

            var setMenuModulePermissionsHandler = new SetMenuItemPermissionsHandler(repositoryMock.Object);

            Assert.Throws<Exception>(() => setMenuModulePermissionsHandler.Handle(command));
        }

        [Test]
        public void Should_update_menu()
        {
            var command = new SetMenuItemPermissions
            {
                SiteId = Guid.NewGuid(),
                MenuId = Guid.NewGuid(),
                MenuItemId = Guid.NewGuid()
            };

            var menu = MenuFactory.Menu(command.SiteId, command.MenuId, "Name", "Item", "Item", command.MenuItemId);

            var repositoryMock = new Mock<IMenuRepository>();
            repositoryMock.Setup(x => x.GetById(command.SiteId, command.MenuId)).Returns(menu);

            var setMenuModulePermissionsHandler = new SetMenuItemPermissionsHandler(repositoryMock.Object);

            setMenuModulePermissionsHandler.Handle(command);

            repositoryMock.Verify(x => x.Update(It.IsAny<Menu>()));
        }
    }
}