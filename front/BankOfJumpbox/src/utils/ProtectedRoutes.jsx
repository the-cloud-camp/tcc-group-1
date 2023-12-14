import { Outlet, Navigate } from 'react-router-dom'
import Header from '../components/layout/header-employee'

const ProtectedRoutes = () => {
    let auth = {'token':true}
    return(
        auth.token ? <><Header /><Outlet/></> : <Navigate to="/login"/>
    )
}

export default ProtectedRoutes