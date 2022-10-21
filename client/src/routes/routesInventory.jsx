import React from "react";
import { Routes, Route, BrowserRouter, Navigate } from "react-router-dom";
import { Footer } from "../components/layout/Footer";
import { Header } from "../components/layout/Header";
import { Nav } from "../components/layout/Nav";
import { Sidebar } from "../components/layout/Sidebar";
import { CreateArticle } from "../components/pages/CreateArticle";
import { Error } from "../components/pages/Error";
import { Index } from "../components/pages/Index";
import { Articles } from "../components/pages/Articles";
import { EditArticle } from "../components/pages/EditArticle";
import { FilterArticle } from "../components/pages/FilterArticle";

export const RoutesInventory = () => {
    return (
        <BrowserRouter>
            {/* Layout */}
            <Header />
            <Nav />
            
            {/* Contenido */}
            <section id="content" className="content">
                <Routes>
                    <Route path="/" element={<Index/>} />
                    <Route path="/home" element={<Index/>} />
                    <Route path="/articles" element={<Articles />} />
                    <Route path="/create-article" element={<CreateArticle/>} />    
                    <Route path="/edit-article/:id" element={<EditArticle/>} />                    
                    <Route path="/filter-article/:textFiltered" element={<FilterArticle/>} />                                                       
                    <Route path="*" element={<Error/>} />
                </Routes>
            </section>

            {/* Side bar */}
            <Sidebar />

            {/* Footer */}
            <Footer />
        </BrowserRouter>
    )
}