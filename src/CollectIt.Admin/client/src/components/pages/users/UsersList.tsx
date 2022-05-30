import React, { useEffect, useState} from 'react';
import User from "../../entities/user";
import Pagination from "../../UI/Pagination/Pagination";
import {UsersService} from "../../../services/UsersService";
import {useNavigate} from "react-router";
import SearchPanel from "../../UI/SearchPanel/SearchPanel";
import ReactLoading from "react-loading";
import "../../../styles/ListStyle.css";

const UsersList = () => {
    const pageSize = 10;

    const [users, setUsers] = useState<User[]>([]);
    const [maxPages, setMaxPages] = useState(0);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        UsersService.getUsersPagedAsync({pageSize, pageNumber: 1}).then(x => {
                setUsers(x.users);
                setMaxPages(Math.ceil(x.totalCount / pageSize));
                setLoading(false);
            }).catch(_ => setLoading(false))
    }, [])

    const downloadPageNumber = (pageNumber: number) => {
        setLoading(true);
        UsersService.getUsersPagedAsync({pageSize, pageNumber}).then(x => {
                setUsers(x.users);
                setLoading(false)
            }).catch(_ => setLoading(false))
    }

    const nav = useNavigate();
    const toEditUserPage = (id: number) => nav(`/users/${id}`);

    const emailRegex = /^\w+([.-]?\w+)*@\w+([.-]?\w+)*(\.\w{2,3})+$/;

    const onSearchEmail = (email: string) => {
        if (!emailRegex.test(email)) {
            return;
        }
        UsersService.findUserByEmailAsync(email).then(u => {
            console.log(u);
            toEditUserPage(u.id);
        }).catch(err => {
            console.error(err);
        });
    }

    const onSearchUsername = (username: string) => {
        if (username === '') {
            downloadPageNumber(1);
            return;
        }
        UsersService.searchUsersByUsernameEntry(username).then(u => {
            setUsers(u);
        }).catch(err => {
            alert(err);
            console.error(err);
        })
    }

    const onSearchId = (q: string) => {
        const id = Number(q);
        if (!Number.isInteger(id)) {
            alert('Id must be an integer');
            return;
        }
        toEditUserPage(id);
    }

    return (
        <div className={'container mt-5'}>
            {loading
                ? <><ReactLoading className={'mx-auto'} type={'spinningBubbles'} color={'black'} height='200px' width='200px' /></>
                : <>
            <SearchPanel onSearch={onSearchId} placeholder={'Enter id'}/>
            <SearchPanel onSearch={onSearchEmail} placeholder={'Enter email'}/>
            <SearchPanel onSearch={onSearchUsername} placeholder={'Enter username'}/>

            <div className='mt-5 mx-auto'>
                <table className={'usersTable table table-borderless table-light'}>
                    <thead>
                    <th className='firstRow usersRow'>
                        <td className='Cell w-10'>ID</td>
                        <td className='Cell w-35'>Username</td>
                        <td className='Cell w-35'>E-mail</td>
                        <td className='Cell'>Roles</td>
                        <td className='Cell'>Banned</td>
                    </th>
                    </thead>
                    <tbody className='mx-auto mt-5 table-hover'>
                    {users?.map(i =>
                            <tr onClick={() => toEditUserPage(i.id)} className='usersRow'>
                                <td className='Cell w-10'>{i.id}</td>
                                <td className='Cell w-35'><div className={'bigtext'}> {i.username}</div></td>
                                <td className='Cell w-35'>{i.email}</td>
                                <td className='Cell'>{i.roles}</td>
                                <td className='Cell'>{i.lockout ? '+' : '' }</td>
                            </tr>
                        )}
                    </tbody>
                </table>
            </div>
                </>
            }
            <footer className={'footer fixed-bottom d-flex mb-0 justify-content-center'}>
                <Pagination totalPagesCount={maxPages} onPageChange={downloadPageNumber}/>
            </footer>
        </div>
    );
};

export default UsersList;