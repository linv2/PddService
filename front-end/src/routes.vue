<script>
import Login from './views/Login'
import LogOut from './views/logout'
import NotFound from './views/404'
import Home from './views/Home'
import Main from './views/main'
import MallList from './views/mall/list'
import MallConfig from './views/mall/config'
import UpdatePassword from './views/users/password'
import UserInfo from './views/users/info'
import OrderList from './views/order/list'
import OrderWaybill from './views/order/waybill'
import OrderPrint from './views/order/print'
import OrderRefund from './views/order/refund';

import PrintTemplate from './views/print/template'

import BindResult from './views/BindResult'

import AddressList from './views/address/list'
import AddressAdd from './views/address/add'
import AddressEdit from './views/address/edit'
import ExpressList from './views/express/list'
import TaskList from './views/task/list'

let routes = [{
        path    : '/',
        redirect: '/login',
    }, {
        path     : '/login',
        component: Login,
        name     : '',
        hidden   : true
    }, {
        path     : '/',
        component: Home,
        iconCls  : 'fa fa-home',
        leaf     : true,
        children : [{
            path     : '/main',
            component: Main,
            name     : '主面板'
        }]
    },
    {
        path     : '/',
        component: Home,
        name     : '',
        iconCls  : 'fa fa-shopping-bag',
        leaf     : true,
        children : [{
                path     : '/mall/list',
                component: MallList,
                name     : '店铺列表'
            },
            {
                path     : '/mall/config/:id',
                component: MallConfig,
                hidden   : true,
                name     : '店铺配置'
            }

        ]
    }, {
        path     : '/',
        component: Home,
        name     : '',
        iconCls  : 'fa fa-shopping-basket',
        leaf     : true,
        children : [{
            path     : '/order/waybill',
            component: OrderWaybill,
            name     : '待发订单'
        }]
    }, {
        path     : '/',
        component: Home,
        name     : '',
        iconCls  : 'fa fa-print',
        leaf     : true,
        children : [{
            path     : '/order/print',
            component: OrderPrint,
            name     : '待打订单'
        }]
    }, {
        path     : '/',
        component: Home,
        name     : '',
        iconCls  : 'fa fa-list',
        leaf     : true,
        children : [{
            path     : '/order/list',
            component: OrderList,
            name     : '全部订单'
        }]
    }, {
        path     : '/',
        component: Home,
        name     : '',
        iconCls  : 'fa fa-reply',
        leaf     : true,
        children : [{
            path     : '/order/refund',
            component: OrderRefund,
            name     : '退款订单'
        }]
    },{
        path     : '/',
        component: Home,
        name     : '',
        iconCls  : 'fa fa-tasks',
        leaf     : true,
        children : [{
            path     : '/task/list',
            component: TaskList,
            name     : '任务列表'
        }]
    },

    {
        path     : '/',
        component: Home,
        name     : '',
        iconCls  : 'fa fa-truck',
        leaf     : true,
        children : [{
            path     : '/print/template',
            component: PrintTemplate,
            name     : '面单模板'
        }, {
            path     : '/express/list',
            component: ExpressList,
            name     : '面单模板'
        }]
    }, {
        path     : '/',
        component: Home,
        name     : '',
        iconCls  : 'fa fa-map-marker',
        leaf     : true,
        children : [{
            path     : '/address/list',
            component: AddressList,
            name     : '发货地址'
        }, {
            path     : '/address/add',
            component: AddressAdd,
            name     : '添加发货地址'
        }, {
            path     : '/address/edit/:id',
            component: AddressEdit,
            name     : '编辑发货地址'
        }]
    }, {
        path     : '/404',
        component: NotFound,
        name     : '',
        hidden   : true
    }, {
        path     : '/',
        component: Home,
        name     : '',
        iconCls  : 'fa fa-user',
        leaf     : true,
        children : [{
            path     : '/user/info',
            component: UserInfo,
            name     : '个人信息'
        }, {
            path     : '/user/passowrd',
            component: UpdatePassword,
            name     : '密码修改'
        }]
    }, {
        path     : '/bind',
        component: BindResult,
        hidden   : true
    }, {
        path    : '*',
        hidden  : true,
        redirect: {
            path: '/404'
        }
    }
]

export default routes
</script>
